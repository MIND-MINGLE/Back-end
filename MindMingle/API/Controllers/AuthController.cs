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

	}
}
