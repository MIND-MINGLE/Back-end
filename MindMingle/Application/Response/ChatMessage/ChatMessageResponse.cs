using System;
namespace Application.Response.ChatMessage
{
	public class ChatMessageResponse
	{
		required public string AccountId { get; set; }
        required public string Content { get; set; }
    }
}

