using Application.Interface;
using Application.Request.Answer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private IAnswerService answerService;
        public AnswerController(IAnswerService answerService)
        {
            this.answerService = answerService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAnswer([FromBody] NewAnswerRequest request)
        {
            var result = await answerService.AddNewAnswer(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
