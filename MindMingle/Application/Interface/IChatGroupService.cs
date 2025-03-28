using System;
using System.Text.RegularExpressions;
using Application.Request.ChatGroupRequest;
using Application.Response;

namespace Application.Interface
{
	public interface IChatGroupService
	{
		public Task<ApiResponse> GetAllChatGroup();
        public Task<ApiResponse> GetAllChatGroupByAdminId(string adminId);
        public Task<ApiResponse> DisableChatGroup(string groupId);
        public Task<ApiResponse> GetChatGroupByGroupId(string groupId);
        public Task<ApiResponse> AddChatGroup(AddChatGroupRequest addChatGroupRequest);
        public Task<ApiResponse> DeleteChatGroup(string groupId);
    }
}

