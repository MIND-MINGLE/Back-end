using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request;
using Application.Response;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Http;
using Twilio.Jwt.AccessToken;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/Twilio")]
    public class TwilioController : ControllerBase
    {
        private readonly ITwilioService twilioService;

        public TwilioController(ITwilioService twilioService)
        {
            this.twilioService = twilioService;
        }

        [HttpPost("CallRoomToken")]
        public async Task<IActionResult> SendCallRoomToken([FromBody]TwilioRequest twilioRequest)
        {
            var response = await twilioService.GetCallRoomToken(twilioRequest.Identity, twilioRequest.Room);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

