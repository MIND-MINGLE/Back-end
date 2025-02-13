using Application.Interface;
using Application.Request.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public IAuthService _service;
		public AuthController(IAuthService service)
		{
			_service = service;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginRequest user)
		{
			var result = await _service.LoginAsync(user);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(UserRegisterRequest user)
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return BadRequest(new
				{
					statusCode = 400,
					isSuccess = false,
					errorMessage = string.Join("; ", errors),
					result = (object)null
				});
			}
			var result = await _service.RegisterAsync(user);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}
		[HttpPost("verification")]
		public async Task<IActionResult> Verification(VerificationEmailRequest request)
		{

			var result = await _service.VerifyEmailAsync(request.AccountId, request.VerificationCode);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

	}
}
