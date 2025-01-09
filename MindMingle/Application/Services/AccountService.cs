using System;
using Application.Interface;
using Application.IRepository;
using Application.Response;
using AutoMapper;
using Domain.Entity;

namespace Application.Services
{
	public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IAccountRepository accountRepository;
        private readonly ApiResponse apiResponse;

        public AccountService(IMapper mapper, IAccountRepository accountRepository,ApiResponse apiResponse)
		{
            this.mapper = mapper;
            this.accountRepository = accountRepository;
            this.apiResponse = apiResponse;
        }

        public async Task<ApiResponse> GetAllAccounts()
        {
            var accountModel = await accountRepository.GetAllAsync(null);
            var resAccount = mapper.Map<List<ResponseAccount>>(accountModel);
            return apiResponse.SetOk(resAccount);
        }
	}
}

// TOFIX
// Cannot call IRepos directly, need to go through unitofwork ... FIX LATER

