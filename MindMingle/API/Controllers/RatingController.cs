using Application.Interface;
using Application.Request.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRatingAsync([FromBody] RatingRequest request)
        {
            var response = await _ratingService.AddRatingAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRatingAsync()
        {
            var response = await _ratingService.GetAllRatingAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetRatingByAppointmentIdAsync(string appointmentId)
        {
            var response = await _ratingService.GetRatingByAppointmentIdAsync(appointmentId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetRatingByPatientIdAsync(string patientId)
        {
            var response = await _ratingService.GetRatingByPatientIdAsync(patientId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
