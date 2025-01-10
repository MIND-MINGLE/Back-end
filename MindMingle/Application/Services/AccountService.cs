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
        private readonly IUnitOfWorks unitOfWorks;
       

        public AccountService(IMapper mapper, IUnitOfWorks unitOfWorks)
		{
            this.mapper = mapper;
            this.unitOfWorks = unitOfWorks;
        }

        public async Task<ApiResponse> GetAllAccounts()
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var accountModel = await unitOfWorks.AccountRepo.GetAllAsync(null);
                 var resAccount = mapper.Map<List<ResponseAccount>>(accountModel);
                // Testing Without Mapper
                //var resAccount = accountModel.Select(account => new ResponseAccount
                //{
                //    // Manually map properties here
                //    AccountName = account.AccountName,
                //    AccountId = account.AccountId
                //    // Add other properties
                //}).ToList();
                Console.WriteLine($"Fetch data complete: {resAccount.Count}");
                return apiResponse.SetOk(resAccount);
            }catch(Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }
	}
}

// TOFIX
// Cannot call IRepos directly, need to go through unitofwork ... FIX LATER

