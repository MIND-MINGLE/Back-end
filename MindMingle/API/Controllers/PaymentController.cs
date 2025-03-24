using Application.Interface;
using Application.Request.Payment;
using Application.Response;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest paymentRequest)
        {
            var response = await _paymentService.CreatePaymentAsync(paymentRequest);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPatch("{paymentId}/status/paid")]
        public async Task<IActionResult> UpdateToPaid(string paymentId)
        {
            var response = await _paymentService.UpdateToPaidAsync(paymentId);
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPatch("{paymentId}/status/canceled")]
        public async Task<IActionResult> UpdateToCanceled(string paymentId)
        {
            var response = await _paymentService.UpdateToCanceledAsync(paymentId);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
