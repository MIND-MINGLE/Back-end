using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request.ChatGroupRequest;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class ChatGroupController : ControllerBase
    {
        private readonly IChatGroupService chatGroupService;

        public ChatGroupController( IChatGroupService chatGroupService)
        {
            this.chatGroupService = chatGroupService;
        }
        // GET: /<controller>/
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllChatGroup()
        {
            var response = await chatGroupService.GetAllChatGroup();

            return response.IsSuccess? Ok(response) : BadRequest(response);
        }
        [HttpGet("admin/{adminId}")]
        public async Task<IActionResult> GetAllChatGroupByAdminId([FromRoute] string adminId)
        {
            var response = await chatGroupService.GetAllChatGroupByAdminId(adminId);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateChatGroup([FromBody] AddChatGroupRequest addChatGroupRequest)
        {
            var response = await chatGroupService.AddChatGroup(addChatGroupRequest);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
      
    }
}

