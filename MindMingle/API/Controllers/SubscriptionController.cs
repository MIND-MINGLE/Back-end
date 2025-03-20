using Application.Interface;
using Application.Request.Subcription;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subcriptionService;

        public SubscriptionController(ISubscriptionService subcriptionService)
        {
            _subcriptionService = subcriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSubscriptionAsync([FromBody] SubscriptionRequest subRequest)
        {
            var response = await _subcriptionService.AddSubscriptionAsync(subRequest);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{subcriptionId}")]
        public async Task<IActionResult> DisableSubscriptionAsync([FromRoute] string subscriptionId)
        {
            var response = await _subcriptionService.DisableSubscriptionAsync(subscriptionId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriptionAsync()
        {
            var response = await _subcriptionService.GetSubscriptionAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{subscriptionId}")]
        public async Task<IActionResult> GetSubscriptionByPatientIdAsync([FromRoute] string subscriptionId)
        {
            var response = await _subcriptionService.GetSubscriptionByIdAsync(subscriptionId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
