using System;
using Application.Response;

namespace Application.Interface
{
	public interface IChatMessageService
	{
        public Task<ApiResponse> GetAllChatMessage();
        public Task<ApiResponse> GetAllChatMessageByAccountId();
        public Task<ApiResponse> AddChatMessageByAccountId();
    }
}

