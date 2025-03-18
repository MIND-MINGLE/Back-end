using Application.Interface;
using Application.Request.Credential;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialService _credentialService;

        public CredentialController(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        [HttpPost("add-credentials")]
        public async Task<IActionResult> AddNewCredentials(CredentialRequest newCredentials)
        {
            var result = await _credentialService.AddNewCredentials(newCredentials);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
