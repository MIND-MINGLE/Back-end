using System;
namespace Application.Request.ChatMessage
{
	public class ChatMessageRequest
	{
        required public string AccountId { get; set; }
        required public string UsersInGroupId { get; set; }
        required public string Content { get; set; }
        public string? MessageStatus { get; set; }
    }
}

