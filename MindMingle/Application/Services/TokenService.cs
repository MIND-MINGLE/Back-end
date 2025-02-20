using Application.Response;
using Domain;
using Domain.Entity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public interface ITokenService
	{
		string GetToken(string token);
		string CreateToken(Account account);
	}

	public class TokenService : ITokenService
	{
		private AppSetting _appSettings;


		public TokenService(AppSetting appSettings)
		{
			_appSettings = appSettings;
		}

		public string CreateToken(Account user)
		{
				List<Claim> claims = new List<Claim>
				{
					new Claim("UserId", user.AccountId.ToString()),
					new Claim("Role", user.RoleId.ToString()),
				};

				var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings!.SecretToken.Value));
				var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

				var token = new JwtSecurityToken(
					claims: claims,
					expires: DateTime.UtcNow.AddHours(2), // Use UtcNow for consistency
					signingCredentials: creds);

				var jwt = new JwtSecurityTokenHandler().WriteToken(token);

				return jwt;	
			}

		public string GetToken(string token)
		{
			throw new NotImplementedException();
		}
	}
}
