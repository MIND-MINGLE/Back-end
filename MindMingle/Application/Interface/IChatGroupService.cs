using System;
using Application.Response;

namespace Application.Interface
{
	public interface IChatGroupService
	{
		public Task<ApiResponse> GetAllChatGroup();
        public Task<ApiResponse> GetAllChatGroupByAdminId(string adminId);
        public Task<ApiResponse> GetChatGroupByGroupId(string groupId);
        public Task<ApiResponse> AddChatGroup();
    }
}

