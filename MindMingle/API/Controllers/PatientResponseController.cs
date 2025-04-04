using Application.Interface;
using Application.Request.PatientResponse;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientResponseController : ControllerBase
    {
        private readonly IPatientResponseService _patientResponseService;

        public PatientResponseController(IPatientResponseService patientResponseService)
        {
            _patientResponseService = patientResponseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddResponse([FromBody] PatientResRequest[] request)
        {
            var response = await _patientResponseService.ComposeResponse(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("survey/{surveyId}")]
        public async Task<IActionResult> GetResponsesBySurveyId(string surveyId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _patientResponseService.GetResponsesBySurveyIdAsync(surveyId, pageIndex, pageSize);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
