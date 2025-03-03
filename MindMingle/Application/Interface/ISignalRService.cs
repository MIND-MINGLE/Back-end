using System;
using Application.Request.ChatMessage;
using Application.Response;

namespace Application.Interface
{
    public interface ISignalRService
    {
		public Task ReceiveTextMessage(ChatMessageRequest chatMessageRequest);
	}
}

