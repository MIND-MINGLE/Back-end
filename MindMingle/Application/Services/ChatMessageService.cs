using System;
using Application.Interface;
using Application.Response;
using AutoMapper;

namespace Application.Services
{
	public class ChatMessageService : IChatMessageService
	{
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper mapper;

        public ChatMessageService(IUnitOfWorks unitOfWorks,IMapper mapper)
		{
            this.unitOfWorks = unitOfWorks;
            this.mapper = mapper;
        }

        Task<ApiResponse> IChatMessageService.AddChatMessageByAccountId()
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse> IChatMessageService.GetAllChatMessage()
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse> IChatMessageService.GetAllChatMessageByAccountId()
        {
            throw new NotImplementedException();
        }
    }
}

