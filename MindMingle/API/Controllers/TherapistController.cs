using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request.Account;
using Application.Request.Therapist;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class TherapistController : ControllerBase
    {
        private readonly ITherapistService therapistservice;

        public TherapistController(ITherapistService therapistservice)
        {
            this.therapistservice = therapistservice;
        }
        [HttpPost("addtherapist")]
        public async Task<IActionResult> AddNewTherapist([FromBody] AddNewTherapistRequest addNewTherapistRequest)
        {
            var response = await therapistservice.AddNewTherapist(addNewTherapistRequest);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{accountId}")]
        public async Task<IActionResult> FindTherapistByAccountId([FromRoute] string accountId)
        {
            var response = await therapistservice.GetTherapistByAccountIdAsync(accountId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpGet("therapist/{therapistId}")]
        public async Task<IActionResult> FindTherapistByTherapistId([FromRoute] string therapistId)
        {
            var response = await therapistservice.GetTherapistByTherapistIdAsync(therapistId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllTherapist()
        {
            var response = await therapistservice.GetAllTherapistAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateTherapist([FromBody] UpdateTherapistRequest request)
        {
            var response = await therapistservice.UpdateTherapistAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

