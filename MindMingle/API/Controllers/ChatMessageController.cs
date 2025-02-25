using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request.ChatMessage;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatMessageService chatMessageService;
        private readonly IHubContext<SignalRService> _hubContext;

        public ChatMessageController(IChatMessageService chatMessageService, IHubContext<SignalRService> hubContext)
        {
            this.chatMessageService = chatMessageService;
            _hubContext = hubContext;
        }
        [HttpPost]
        public async Task<IActionResult> AddChatMessage([FromBody]
        ChatMessageRequest chatMessageRequest)
        {
            var response = await chatMessageService.AddChatMessageByAccountId(chatMessageRequest);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpGet("getchatlog/{usersInGroupId}")]
        public async Task<IActionResult> GetChatMessage([FromRoute]string usersInGroupId)
        {
            var response = await chatMessageService.GetAllChatMessageByGroupId(usersInGroupId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTextMessage", "Test message from API");
            return Ok("Hub Test Triggered");
        }
    }
}

