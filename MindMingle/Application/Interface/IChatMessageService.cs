using System;
using Application.Request.ChatMessage;
using Application.Response;

namespace Application.Interface
{
	public interface IChatMessageService
	{
        public Task<ApiResponse> GetAllChatMessageByGroupChatId(string chatGroupId);
        public Task<ApiResponse> GetAllChatMessageByGroupId(string usersInGroupChatId);
        public Task<ApiResponse> AddChatMessageByAccountId(ChatMessageRequest chatMessageRequest);
    }
}

