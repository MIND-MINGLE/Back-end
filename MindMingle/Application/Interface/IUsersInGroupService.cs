using System;
using Application.Request.UsersInGroup;
using Application.Response;

namespace Application.Interface
{
	public interface IUsersInGroupService
	{
        public Task<ApiResponse> GetAllGroupChatWithUser();
        public Task<ApiResponse> GetGroupChatListByClientId(string accountId);
        public Task<ApiResponse> GetAllUserInGroup(string groupId);
        public Task<string> GetGroupChatByUsersInGroup(string userInGroupId);
        public Task<ApiResponse> AddUsersIntoGroup(UsersInGroupRequest usersInGroupRequest);

    }
}

