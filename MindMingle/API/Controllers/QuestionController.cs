using Application.Interface;
using Application.Request.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequest request)
        {
            var result = await _questionService.AddNewQuestion(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteQuestion([FromBody] string questionId)
        {
            var result = await _questionService.DeleteQuestion(questionId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("question")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var result = await _questionService.GetAllQuestions();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> GetQuestionById(string questionId)
        {
            var result = await _questionService.GetQuestionById(questionId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
