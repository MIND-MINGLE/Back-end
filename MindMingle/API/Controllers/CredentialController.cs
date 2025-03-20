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

        [HttpPost]
        public async Task<IActionResult> AddNewCredentials(CredentialRequest newCredentials)
        {
            var result = await _credentialService.AddNewCredentials(newCredentials);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{therapistId}")]
        public async Task<IActionResult> GetCredentialsByTherapistId(string therapistId)
        {
            var result = await _credentialService.GetCredentialsByTherapistId(therapistId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{credentialId}")]
        public async Task<IActionResult> UpdateCredentails(string credentialId, [FromBody] UpdateCredentialRequest updateCredentialRequest)
        {
            var result = await _credentialService.UpdateCredentails(credentialId, updateCredentialRequest);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{credentailId}")]
        public async Task<IActionResult> DisableCredentials(string credentailId)
        {
            var result = await _credentialService.DisableCredentials(credentailId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
