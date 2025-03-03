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

        public async Task<ApiResponse> GetAllChatMessageByGroupChatId(string chatGroupId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var groupChatExist = await unitOfWorks.ChatGroupRepo.GetAllAsync(g => g.Id == chatGroupId);
                if (groupChatExist.Count == 0)
                {
                    return response.SetNotFound("No Group Chat Exist");
                }

                // Get all UsersInGroup records that belong to the given ChatGroupId
                var usersInGroup = await unitOfWorks.UsersInGroupRepo
                    .GetAllAsync(ug => ug.ChatGroupId == chatGroupId);

                // Extract UsersInGroup IDs
                var usersInGroupIds = usersInGroup.Select(ug => ug.UsersInGroupId).ToList();

                // Find and sort chat messages where UsersInGroupId matches
                var chatMessageModel = await unitOfWorks.ChatMessageRepo
                    .GetAllAsync(cm => usersInGroupIds.Contains(cm.UsersInGroupId));

                // Sort messages by CreatedAt (oldest to newest) - consistent data loading
                var sortedChatMessages = chatMessageModel.OrderBy(cm => cm.CreatedAt).ToList();

                var chatMessageResponse = mapper.Map<ChatMessageResponse[]>(sortedChatMessages);
                return response.SetOk(chatMessageResponse);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
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

