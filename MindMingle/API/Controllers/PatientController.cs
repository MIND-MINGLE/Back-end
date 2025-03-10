using Application.Interface;
using Application.Request.Patient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private IPatientService _patientService;
		public PatientController(IPatientService patientService)
		{
			_patientService = patientService;
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreatePatient([FromBody] CreateNewPatientRequest request)
		{
			var result = await _patientService.AddNewPatient(request);
			if(!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("patient/{accountId}")]
		public async Task<IActionResult> GetPatientByAccountId(string accountId)
		{
			var result = await _patientService.GetPatientByAccountIdAsync(accountId);
			if(!result.IsSuccess)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

        [HttpGet("patient")]
        public async Task<IActionResult> GetAllPatients()
        {
            var result = await _patientService.GetAllPatientsAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
