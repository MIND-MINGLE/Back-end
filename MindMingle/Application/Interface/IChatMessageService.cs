using System;
using Application.Request.ChatMessage;
using Application.Response;

namespace Application.Interface
{
	public interface IChatMessageService
	{
        public Task<ApiResponse> GetAllChatMessage();
        public Task<ApiResponse> GetAllChatMessageByGroupId(string usersInGroupChatId);
        public Task<ApiResponse> AddChatMessageByAccountId(ChatMessageRequest chatMessageRequest);
    }
}

