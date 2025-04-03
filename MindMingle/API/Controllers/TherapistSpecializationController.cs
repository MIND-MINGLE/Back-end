using Application.Interface;
using Application.Request.Therapist_Specialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistSpecializationController : ControllerBase
    {
        private readonly ITherapistSpecializationService _therapistSpecializationService;

        public TherapistSpecializationController(ITherapistSpecializationService therapistSpecializationService)
        {
            _therapistSpecializationService = therapistSpecializationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTherapistSpecializationAsync(TherapistSpecializationRequest request)
        {
            var response = await _therapistSpecializationService.AddTherapistSpecializationAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTherapistSpecializationAsync()
        {
            var response = await _therapistSpecializationService.GetTherapistSpecializationAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{therapistId}")]
        public async Task<IActionResult> GetTherapistSpecializationByIdAsync(string therapistId)
        {
            var response = await _therapistSpecializationService.GetTherapistSpecializationByTherapistIdAsync(therapistId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DisableTherapistSpecializationByIdAsync(string id)
        {
            var response = await _therapistSpecializationService.DisableTherapistSpecializationByIdAsync(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
