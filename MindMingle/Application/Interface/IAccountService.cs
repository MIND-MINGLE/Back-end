using System;
using Application.Request.Account;
using Application.Response;

namespace Application.Interface
{
	public interface IAccountService
	{
        public Task<ApiResponse> GetAllAccounts();
		public Task<ApiResponse> GetAccountById(string accountId);
		public Task<ApiResponse> UpdateAvatarAsync(AvatarRequest newAvatar);
        
    }
}

