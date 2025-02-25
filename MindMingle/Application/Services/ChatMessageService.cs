using System;
using Application.Interface;
using Application.Request.ChatMessage;
using Application.Response;
using Application.Response.ChatMessage;
using AutoMapper;
using Domain.Entity;

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

        public async Task<ApiResponse> AddChatMessageByAccountId(ChatMessageRequest chatMessageRequest)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var groupChatExist = await unitOfWorks.UsersInGroupRepo.GetAllAsync(g => g.UsersInGroupId == chatMessageRequest.UsersInGroupId);
                if(groupChatExist.Count == 0)
                {
                    return response.SetNotFound("No Group Chat Exist");
                }
                var chatMessageModel = mapper.Map<ChatMessage>(chatMessageRequest);
                chatMessageModel.ChatMessageId = Guid.NewGuid().ToString();
                await unitOfWorks.ChatMessageRepo.AddAsync(chatMessageModel);
                await unitOfWorks.SaveChangeAsync();
                return response.SetOk(chatMessageRequest);

            }
            catch(Exception ex)
            {
                 return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }

        public async Task<ApiResponse> GetAllChatMessage()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetAllChatMessageByGroupId(string usersInGroupId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var groupChatExist = await unitOfWorks.UsersInGroupRepo.GetAllAsync(g => g.UsersInGroupId == usersInGroupId);
                if (groupChatExist.Count == 0)
                {
                    return response.SetNotFound("No Group Chat Exist");
                }
                var chatMessageModel = await unitOfWorks.ChatMessageRepo.GetAllAsync(cm => cm.UsersInGroupId == usersInGroupId);
                var chatMessageResponse = mapper.Map<ChatMessageResponse[]>(chatMessageModel);
                return response.SetOk(chatMessageResponse);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }
    }
}

