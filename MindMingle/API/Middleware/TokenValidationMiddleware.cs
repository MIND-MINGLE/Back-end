using Application.Interface;
using Application.Services;
using Domain.Entity;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Twilio.Http;

namespace API.Middleware
{
	public class TokenValidationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ITokenService _tokenService; // Service to create new tokens
		private readonly IAccountService _accountSerivce;

		public TokenValidationMiddleware(RequestDelegate next, ITokenService tokenService, IAccountService accountService)
		{
			_next = next;
			_tokenService = tokenService;
			_accountSerivce = accountService;
		}

		public async Task Invoke(HttpContext context)
		{
			if(!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
			{
				await _next(context);
				return;
			}

			var token = authHeader.ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Token is required !");
				return;
			}

			if(IsTokenExpired(token, out var accountId))
			{
				//Lấy thông tin Account bằng AccountId
				var response = await _accountSerivce.GetAccountById(accountId);
				if (!response.IsSuccess)
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					await context.Response.WriteAsync("Invalid token!");
					return;
				}
				var user = response.Result as Account;  // Ép kiểu sang Account
				if (user == null)
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					await context.Response.WriteAsync("User not found!");
					return;
				}
				// Tạo token mới
				var newToken = _tokenService.CreateToken(user);

				// Gửi token mới về header
				context.Response.Headers.Add("New-Token", newToken);
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

					// 🔹 Lấy AccountId từ token (khớp với CreateToken)
					var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");
					if (userIdClaim != null)
					{
						accountId = userIdClaim.Value;
					}

					return expirationDate < DateTime.UtcNow; // Trả về true nếu token đã hết hạn
				}
			}
			catch
			{
				return true; // Nếu token không hợp lệ, xem như đã hết hạn
			}

			return false;
		}

	}
}
