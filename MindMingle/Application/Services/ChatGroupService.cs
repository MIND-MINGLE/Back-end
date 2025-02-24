using System;
using Application.Interface;
using Application.Request.ChatGroupRequest;
using Application.Request.UsersInGroup;
using Application.Response;
using AutoMapper;
using Domain.Entity;

namespace Application.Services
{
	public class ChatGroupService : IChatGroupService
	{
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper mapper;
        private readonly IUsersInGroupService usersInGroupService;

        public ChatGroupService(IUnitOfWorks unitOfWorks, IMapper mapper, IUsersInGroupService usersInGroupService)
		{
            this.unitOfWorks = unitOfWorks;
            this.mapper = mapper;
            this.usersInGroupService = usersInGroupService;
        }

        public async Task<ApiResponse> AddChatGroup(AddChatGroupRequest addChatGroupRequest)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var chatGroupModel = mapper.Map<ChatGroup>(addChatGroupRequest);
                chatGroupModel.ChatGroupId = Guid.NewGuid().ToString();
                await unitOfWorks.ChatGroupRepo.AddAsync(chatGroupModel);
                await unitOfWorks.SaveChangeAsync();
                // Multi-task that when a new group is created, the admin got joined in too. Saving the time have to call another API in FE
                UsersInGroupRequest newUser = new UsersInGroupRequest()
                {
                    ClientId = chatGroupModel.AdminId,
                    GroupId = chatGroupModel.ChatGroupId
                };
                var addUserInGroup = usersInGroupService.AddUsersIntoGroup(newUser);
                return response.SetOk(chatGroupModel);
            }catch (Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }

        public async Task<ApiResponse> DeleteChatGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetAllChatGroup()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetAllChatGroupByAdminId(string adminId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetChatGroupByGroupId(string groupId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var chatGroupModel = await unitOfWorks.ChatGroupRepo.GetAsync(g=>g.ChatGroupId==groupId);
                if (chatGroupModel != null)
                    return response.SetOk(chatGroupModel);
                else
                    return response.SetNotFound("No Groupchat found!");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }
    }
}

