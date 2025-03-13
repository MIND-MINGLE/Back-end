using Application.Interface;
using Application.Request.PatientSurvey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientSurveyController : ControllerBase
    {
        private readonly IPatientSurveyService _patientSurveyService;

        public PatientSurveyController(IPatientSurveyService patientSurveyService)
        {
            _patientSurveyService = patientSurveyService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSurvey([FromBody] PatientSurveyRequest request)
        {
            var response = await _patientSurveyService.AddSurveyAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetSurveysByPatientId(string patientId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _patientSurveyService.GetSurveysByPatientIdAsync(patientId, pageIndex, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
