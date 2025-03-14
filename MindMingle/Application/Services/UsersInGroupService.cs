using System;
using System.Text.RegularExpressions;
using Application.Interface;
using Application.Request.UsersInGroup;
using Application.Response;
using Application.Response.ChatGroup;
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
                if (userExist.Count != 0)
                {
                    return response.SetBadRequest("User Is Already In The Group Chat");
                }
                userInGroupModel.UsersInGroupId = Guid.NewGuid().ToString();
                Console.WriteLine($"User Model: {userInGroupModel.ChatGroupId} + {userInGroupModel.ClientId}");
                await unitOfWorks.UsersInGroupRepo.AddAsync(userInGroupModel);
                await unitOfWorks.SaveChangeAsync();
                return response.SetOk(userInGroupModel);
            }
            catch (Exception ex)
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
                   include: query => query.Include(a => a.Accounts) // This is a custom JOIN operation to get the account Name of all user in group
                   );
                var userInGroupResponse = mapper.Map<GetAllUserInGroupResponse[]>(userInGroupModel);
                foreach (var user in userInGroupModel)
                {
                    Console.WriteLine($"User ID: {user.Accounts?.AccountId}, Account: {user.Accounts?.AccountName}");
                }
                return response.SetOk(userInGroupResponse);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }

        public async Task<ApiResponse> GetGroupChatListByClientId(string AccountId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userInGroupModel = await unitOfWorks.UsersInGroupRepo.GetAllAsync(ug => ug.ClientId == AccountId);
                if (userInGroupModel.Count == 0)
                {
                    return response.SetNotFound("Empty Group Chat");
                }
                else
                {
                    // There is no way I am doing extra 3 repos DB calling just to fetch a NAME for therapist admin. I'm gonna use LINQ - Language-Integrated Query of EFCore
                    var chatgroupList = userInGroupModel.Select(cg => cg.ChatGroupId).ToList();
                    var chatGroupResponses = await unitOfWorks.ChatGroupRepo.GetAllAsync(cg => chatgroupList.Contains(cg.Id));
                    //Console.WriteLine("chatGroupResponses: ", chatGroupResponses);
                    var therapistList = await unitOfWorks.TherapistRepo.GetAllAsync(null); // Await this before using
                    var chatGroupWithAdmins = from cg in chatGroupResponses
                                              //join t in therapistList on cg.AdminId equals t.AccountId
                                              join ug in userInGroupModel on cg.Id equals ug.ChatGroupId
                                              select new ChatGroupResponse
                                              {
                                                  ChatGroupId = cg.Id,
                                                  AdminId = cg.AdminId,
                                                  AdminName = "Test", //t.FirstName + " " + t.LastName,
                                                  UserInGroupId = ug.UsersInGroupId
                                              };


                    var result = chatGroupWithAdmins.ToList();
                    return response.SetOk(result.Count == 0 ? "No Therapy Exist" : result);
                }

            }
            catch (Exception ex)
            {
                return response.SetBadRequest($"Error: {ex.Message}. Details: {ex.InnerException?.Message}");
            }
        }

       public async Task<string> GetGroupChatByUsersInGroup(string userInGroupId)
        {
            ApiResponse response = new ApiResponse();
            var chatgroupId = await unitOfWorks.UsersInGroupRepo.GetAsync(ug => ug.UsersInGroupId == userInGroupId);
            if (chatgroupId!=null)
            {
                return chatgroupId.ChatGroupId;
            }
            else
            {
                return "";
            }
           
        }
    }
}

