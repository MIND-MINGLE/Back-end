using Application.Interface;
using Application.Request.Account;
using Application.Response;
using Domain;
using Domain.Entity;
using Microsoft.AspNetCore.Identity; // PasswordHasher<Account>
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class AuthService : IAuthService
	{
		private IUnitOfWorks _unitOfWorks;
		private AppSetting _appSettings;
		//private IClaimService _claimService;
		private IPasswordHasher<Account> _passwordHasher;

		public AuthService(IUnitOfWorks unitOfWorks, AppSetting appSettings, IPasswordHasher<Account> passwordHasher)
		{
			_unitOfWorks = unitOfWorks;
			_appSettings = appSettings;
			_passwordHasher = passwordHasher;
		}

		public async Task<ApiResponse> LoginAsync(LoginRequest request)
		{
			ApiResponse response = new ApiResponse();
			var user = await _unitOfWorks.AccountRepo.GetAsync(x => x.Email == request.Email || x.AccountName == request.AccountName);
			if (user == null)
			{
				response.SetBadRequest(message: "Invalid AccountName/Email or Password");
				return response;
			}
			//var password = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
			//if (password != PasswordVerificationResult.Success)
			//{
			//	response.SetBadRequest(message: "Invalid Password");
			//	return response;
			//}

			if(user.Password != request.Password)
			{
				response.SetBadRequest(message: "Invalid Password");
				return response;
			}
			//var claims =  _claimService.GetClaim();
			var token = CreateToken(user);
			response.SetOk(token);
			return response;
		}

		public Task<ApiResponse> LoginForDriverAsync(LoginRequest request)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> RegisterAsync(UserRegisterRequest userRequest)
		{
			throw new NotImplementedException();
		}

		private string CreateToken(Account user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim("UserId", user.AccountId.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
				new Claim("Role", user.RoleId.ToString()),
				new Claim(ClaimTypes.Role, user.RoleId.ToString()),
				new Claim( "Email" , user.Email ?? string.Empty),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim("Username", user.AccountName ?? string.Empty),
				new Claim(ClaimTypes.Name, user.AccountName),
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
				 _appSettings!.SecretToken.Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;
		}

	}

}
