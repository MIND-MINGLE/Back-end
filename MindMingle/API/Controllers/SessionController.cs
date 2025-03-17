using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request.Question;
using Application.Request.Session;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
        {
            var result = await sessionService.CreateSession(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateSession([FromBody] UpdateSessionRequest request)
        {
            var result = await sessionService.UpdateSession(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("delete/{sessionId}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] string sessionId)
        {
            var result = await sessionService.DeleteSession(sessionId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("getAllSessions")]
        public async Task<IActionResult> GetAllSessions()
        {
            var result = await sessionService.GetSession();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("getSessionsByTherapist/{therapistId}")]
        public async Task<IActionResult> GetSessionsByTherapist([FromRoute]string therapistId)
        {
            var result = await sessionService.GetSessionByTherapistId(therapistId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("getSessionById/{sessionId}")]
        public async Task<IActionResult> GetAllSessions([FromRoute] string sessionId)
        {
            var result = await sessionService.GetSessionBySessionId(sessionId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

