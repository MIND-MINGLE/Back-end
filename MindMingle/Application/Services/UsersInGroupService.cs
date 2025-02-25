using System;
using Application.Interface;
using Application.Request.UsersInGroup;
using Application.Response;
using Application.Response.UsersInGroup;
using AutoMapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Twilio.Http;

namespace Application.Services
{
	public class UsersInGroupService : IUsersInGroupService
	{
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper mapper;

        public UsersInGroupService(IUnitOfWorks unitOfWorks, IMapper mapper)
		{
            this.unitOfWorks = unitOfWorks;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> GetAllGroupChatWithUser()
        {
            ApiResponse response = new ApiResponse();
            var data = await unitOfWorks.UsersInGroupRepo.GetAllAsync(null);
            return response.SetOk(data);
        }


       public async Task<ApiResponse> AddUsersIntoGroup(UsersInGroupRequest usersInGroupRequest)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userInGroupModel = mapper.Map<UsersInGroup>(usersInGroupRequest);
                var userExist = await unitOfWorks.UsersInGroupRepo.GetAllAsync(
                    g => g.ClientId == userInGroupModel.ClientId && g.ChatGroupId == userInGroupModel.ChatGroupId
                    );
                Console.WriteLine($"User Model: {userInGroupModel.ChatGroupId} + {userInGroupModel.ClientId}");
                if (userExist.Count!=0)
                {
                    return response.SetBadRequest("User Is Already In The Group Chat");
                }
                userInGroupModel.UsersInGroupId = Guid.NewGuid().ToString();
                Console.WriteLine($"User Model: {userInGroupModel.ChatGroupId} + {userInGroupModel.ClientId}");
                await unitOfWorks.UsersInGroupRepo.AddAsync(userInGroupModel);
                await unitOfWorks.SaveChangeAsync();
                return response.SetOk(userInGroupModel);
            }catch(Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }

        }

        public async Task<ApiResponse> GetAllUserInGroup(string groupId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userInGroupModel = await unitOfWorks.UsersInGroupRepo.GetAllAsync(u => u.ChatGroupId == groupId,
                   include: query => query.Include(a => a.Account) // This is a custom JOIN operation to get the account Name of all user in group
                   );
                var userInGroupResponse = mapper.Map<GetAllUserInGroupResponse>(userInGroupModel);
                return response.SetOk(userInGroupModel);
            }
            catch(Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }

        public async Task<ApiResponse> GetGroupByClientId(string clientId)
        {
            throw new NotImplementedException();
        }
    }
}

