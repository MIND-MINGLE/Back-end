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

        public async Task<ApiResponse> GetGroupChatListByClientId(string accountId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                // Get the caller's group memberships
                var userInGroupModel = await unitOfWorks.UsersInGroupRepo.GetAllAsync(
                    ug => ug.ClientId == accountId,
                    q => q.Include(ug => ug.Accounts) // Include client’s Account
                );

                if (userInGroupModel.Count == 0)
                {
                    return response.SetNotFound("Empty Group Chat");
                }

                // Determine if the caller is a therapist
                var callerAccount = await unitOfWorks.AccountRepo.GetAsync(
                    a => a.AccountId == accountId,
                    q => q.Include(a => a.Therapist) // Include Therapist details if exists
                );
                bool isTherapist = callerAccount?.Therapist != null;

                // Get chat group IDs
                var chatgroupList = userInGroupModel.Select(cg => cg.ChatGroupId).ToList();
                var chatGroupResponses = await unitOfWorks.ChatGroupRepo.GetAllAsync(
                    cg => chatgroupList.Contains(cg.GroupChatId),
                    q => q.Include(cg => cg.Account).ThenInclude(a => a.Therapist) // Admin’s Account and Therapist
                          .Include(cg => cg.UsersInGroups).ThenInclude(ug => ug.Accounts).ThenInclude(a => a.Patient) // Clients’ Accounts and Patient
                );

                var chatGroupWithAdmins = chatGroupResponses.Select(cg =>
                {
                    // For therapist POV, find a patient client (excluding the caller)
                    var patientClient = isTherapist
                        ? cg.UsersInGroups.FirstOrDefault(ug => ug.Accounts.Patient != null && ug.ClientId != accountId)
                        : null;

                    return new ChatGroupResponse
                    {
                        IsDisabled = cg.IsDisabled,
                        ChatGroupId = cg.GroupChatId,
                        AdminId = cg.AdminId,
                        AdminName = isTherapist
                            ? patientClient != null
                                ? patientClient.Accounts.AccountName // Therapist POV: Patient’s AccountName
                                : "No Patient Client" // Fallback
                            : cg.Account.Therapist != null
                                ? $"{cg.Account.Therapist.FirstName} {cg.Account.Therapist.LastName}" // Patient POV: Therapist’s FirstName LastName
                                : cg.Account.AccountName, // Fallback if admin isn’t a therapist
                        UserInGroupId = userInGroupModel.First(ug => ug.ChatGroupId == cg.GroupChatId).UsersInGroupId,
                       
                    };
                }).ToList();

                return response.SetOk(chatGroupWithAdmins.Any() ? chatGroupWithAdmins : new List<ChatGroupResponse>());
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
            if (chatgroupId != null)
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

