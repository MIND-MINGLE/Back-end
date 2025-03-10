using Application.Interface;
using Domain.Entity;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory; // ✅ Use ScopeFactory

        public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                await _next(context);
                return;
            }

            var token = authHeader.ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token is required!");
                return;
            }

            if (IsTokenExpired(token, out var accountId))
            {
                using (var scope = _scopeFactory.CreateScope()) // ✅ Create a new scope
                {
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

                    // ✅ Retrieve Account by ID
                    var response = await accountService.GetAccountById(accountId);
                    if (!response.IsSuccess)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid token!");
                        return;
                    }

                    var user = response.Result as Account;
                    if (user == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("User not found!");
                        return;
                    }

                    // ✅ Generate a new token
                    var newToken = tokenService.CreateToken(user);

                    // ✅ Send new token in response header
                    context.Response.Headers.Add("New-Token", newToken);
                }
            }

            await _next(context);
        }

        private bool IsTokenExpired(string token, out string accountId)
        {
            var handler = new JwtSecurityTokenHandler();
            accountId = null;

            try
            {
                var jwtToken = handler.ReadJwtToken(token);
                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");
                if (expClaim != null)
                {
                    var expirationDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;

                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim != null)
                    {
                        accountId = userIdClaim.Value;
                    }

                    return expirationDate < DateTime.UtcNow; // ✅ Return true if expired
                }
            }
            catch
            {
                return true; // ✅ If token is invalid, treat as expired
            }

            return false;
        }
    }
}
