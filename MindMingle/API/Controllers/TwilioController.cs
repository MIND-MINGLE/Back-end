using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
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

        [HttpGet("CallRoomToken")]
        public async Task<IActionResult> SendCallRoomToken(string identity, string room)
        {
            var response = await twilioService.GetCallRoomToken(identity, room);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

