using Application.Interface;
using Application.Request.Specialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecializationAsync([FromBody] SpecializationRequest request)
        {
            var response = await _specializationService.CreateSpecializationAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{specId}")]
        public async Task<IActionResult> UpdateSpecializationAsync(string specId, [FromBody] SpecializationRequest request)
        {
            var response = await _specializationService.UpdateSpecializationAsync(specId, request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{specId}")]
        public async Task<IActionResult> DisableSpecializationAsync(string specId)
        {
            var response = await _specializationService.DisableSpecializationAsync(specId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{specId}")]
        public async Task<IActionResult> GetSpecializationByIdAsync(string specId)
        {
            var response = await _specializationService.GetSpecializationByIdAsync(specId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializationsAsync()
        {
            var response = await _specializationService.GetAllSpecializationsAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
