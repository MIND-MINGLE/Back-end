using Application.Interface;
using Application.Request.EmergencyEnd;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyEndController : ControllerBase
    {
        private readonly IEmergencyEndService _emergencyEndService;

        public EmergencyEndController(IEmergencyEndService emergencyEndService)
        {
            _emergencyEndService = emergencyEndService;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmergencyEnd([FromBody] EmergencyEndRequest newEmergencyEnd)
        {
            var response = await _emergencyEndService.AddNewEmergencyEnd(newEmergencyEnd);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
