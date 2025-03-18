using Application.Interface;
using Application.Request.Account;
using Application.Response;
using Domain;
using Domain.Entity;
using Microsoft.AspNetCore.Identity; // PasswordHasher<Account>

namespace Application.Services
{ 
	public class AuthService : IAuthService
	{
		private IUnitOfWorks _unitOfWorks;
		private AppSetting _appSettings;
		//private IClaimService _claimService;
		private IEmailService _emailService;
		private ITokenService _tokenService;
		private IPasswordHasher<Account> _passwordHasher;

		public AuthService(IUnitOfWorks unitOfWorks, AppSetting appSettings, IPasswordHasher<Account> passwordHasher, IEmailService emailService, ITokenService tokenService)
		{
			_unitOfWorks = unitOfWorks;
			_appSettings = appSettings;
			_passwordHasher = passwordHasher;
			_emailService = emailService;	
			_tokenService = tokenService;
		}

		public async Task<ApiResponse> LoginAsync(LoginRequest request)
		{
			ApiResponse response = new ApiResponse();
			var user = await _unitOfWorks.AccountRepo.GetAsync(x => x.Email == request.EmailOrAccountName || x.AccountName == request.EmailOrAccountName);
			if (user == null)
			{
				response.SetBadRequest(message: "Invalid AccountName, Email or Password");
				return response;
			}
			var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);
			if (passwordVerificationResult != PasswordVerificationResult.Success)
			{
				response.SetBadRequest(message: "Invalid Password");
				return response;
			}

			user.LastLogin = DateTime.Now;
            await _unitOfWorks.SaveChangeAsync();

            //var claims =  _claimService.GetClaim();
            var token = _tokenService.CreateToken(user);
			response.SetOk(token);
			return response;
		}

		public async Task<ApiResponse> VerifyEmailAsync(string accountId, string verificationCode)
		{
			ApiResponse response = new ApiResponse();

			//Retrieve the verification code 
			var verified = await _unitOfWorks.EmailVerificationRepo.GetAsync(x => x.AccountId == accountId && x.VerificationCode == verificationCode && x.IsUsed == false);

			//Verification record not found
			if(verified == null)
			{
				response.SetBadRequest(message: "Invalid Code!!");
				return response;
			}


			//Check if the code has expired
			if(verified.ExpiresAt < DateTime.Now)
			{
				response.SetBadRequest(message: "The verification code has expired!!");
				return response; 
			}

			//Mark the verification code as verified
			verified.IsUsed = true;
			await _unitOfWorks.SaveChangeAsync();

			//Mark the user's email as verified 
			var user = await _unitOfWorks.AccountRepo.GetAsync(x => x.AccountId == accountId);
			if(user == null)
			{
				response.SetBadRequest(message: "User not found!!");
				return response;
			}

			user.IsEmailVerified = true;
			await _unitOfWorks.SaveChangeAsync();

			response.SetOk("Email verified successfully!!");
			return response;
		}

		public async Task<ApiResponse> RegisterAsync(UserRegisterRequest userRequest,string roleId)
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
					RoleId = roleId,
					AccountId = Guid.NewGuid().ToString(),
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
				EmailVerification emailVerification = new EmailVerification()
				{
					VerificationId = Guid.NewGuid().ToString(),
					AccountId = newAccount.AccountId,
					VerificationCode = verificationCode,
					ExpiresAt = DateTime.Now.AddMinutes(10), //code valid for 10 minutes
					IsUsed = false
				};

				await _unitOfWorks.EmailVerificationRepo.AddAsync(emailVerification);
				await _unitOfWorks.SaveChangeAsync();

				//Prepare email content
				string emailContent = $"Dear {newAccount.AccountName},<br/>Please use the following verification code to validate your email: <strong>{verificationCode}</strong>.<br/>The code will expire in 10 minutes.";


				//Send validation email
				var emailResponse = await _emailService.SendValidationEmail(newAccount.Email, emailContent);
				if (!emailResponse.IsSuccess)
				{
					response.SetBadRequest(message: "Failed to send verification email !!");
					return response;
				}

				response.SetOk(newAccount.AccountId);
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
	}

}
