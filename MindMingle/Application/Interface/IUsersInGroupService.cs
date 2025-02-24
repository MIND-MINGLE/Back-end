using System;
using Application.Response;

namespace Application.Interface
{
	public interface IUsersInGroupService
	{
        public Task<ApiResponse> GetAllUserInGroup(string groupId);
    }
}

