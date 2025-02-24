using System;
using Application.Request.UsersInGroup;
using Application.Response;

namespace Application.Interface
{
	public interface IUsersInGroupService
	{
        public Task<ApiResponse> GetAllUserInGroup(string groupId);
        public Task<ApiResponse> AddUsersIntoGroup(UsersInGroupRequest usersInGroupRequest);
        public Task<ApiResponse> GetGroupByClientId(string clientId);

    }
}

