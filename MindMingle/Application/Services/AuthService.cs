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

		public async Task<ApiResponse> LoginForDriverAsync(LoginRequest request)
		{
			throw new NotImplementedException();
		}

		public async Task<ApiResponse> RegisterAsync(UserRegisterRequest userRequest)
		{
			ApiResponse response = new ApiResponse();
			try
			{
				var checkPassword = CheckUserPassword(userRequest);
				if (!checkPassword){
					response.SetBadRequest(message: "Confirm password is wrong !");
					return response;
				}
				var existedUser = await _unitOfWorks.AccountRepo.GetAsync(x => x.Email == userRequest.Email || x.AccountName == userRequest.AccountName);
				if(existedUser != null)
				{
					response.SetBadRequest(message: "The Email/Account Name is already existed !");
					return response;
				}

				Account newAccount = new Account()
				{
					RoleId = "New",
					AccountId = new Guid().ToString(),
					Email = userRequest.Email,
					AccountName = userRequest.AccountName,
					Password = userRequest.Password,
				};

				//Hash the password
				var hashedPassword = _passwordHasher.HashPassword(newAccount, userRequest.Password);
				newAccount.Password = hashedPassword;

				await _unitOfWorks.AccountRepo.AddAsync(newAccount);
				await _unitOfWorks.SaveChangeAsync();

				//Generate verification code 
				var verificationCode = GenerateVerificationCode();
				//EmailVerification emailVerification = new EmailVerification()
				//{
				//	AccountId = newAccount.AccountId.ToString,

				//}


				response.SetOk();
				return response;
			}
			catch (Exception ex)
			{
				return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
			}
		}
		private string GenerateVerificationCode()
		{
			Random random = new Random();
			return random.Next(100000, 999999).ToString(); // Generate a 6-digit code
		}
		private bool CheckUserPassword(UserRegisterRequest user)
		{
			if (user.Password is null) return false;
			return (user.Password.Equals(user.ConfirmPassword));
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
