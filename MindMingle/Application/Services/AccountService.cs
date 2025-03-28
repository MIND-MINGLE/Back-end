using System;
using Application.Interface;
using Application.IRepository;
using Application.Request.Account;
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

		public async Task<ApiResponse> GetAccountById(string accountId)
		{
            ApiResponse response = new ApiResponse();
            try
            {
                var account = await unitOfWorks.AccountRepo.GetAsync(x => x.AccountId == accountId);
                var resAccount = mapper.Map<Account>(account);  
                return response.SetOk(resAccount);

			} catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);

			}
		}
		public async Task<ApiResponse> GetAllAccounts()
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var accountModel = await unitOfWorks.AccountRepo.GetAllAsync(null);
                 var resAccount = mapper.Map<List<ResponseAccount>>(accountModel); // Error Here
                // Testing Without Mapper
                //var resAccount = accountModel.Select(account => new ResponseAccount
                //{
                //    // Manually map properties here
                //    AccountName = account.AccountName,
                //    AccountId = account.AccountId,
                //    RoleId = account.RoleId,
                //    // Add other properties
                //}).ToList(); // WORK!!!
                Console.WriteLine($"Fetch data complete: {resAccount.Count}");
                return apiResponse.SetOk(resAccount);
            }catch(Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }

        public async Task<ApiResponse> UpdateAvatarAsync(AvatarRequest newAvatar)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (newAvatar == null || string.IsNullOrEmpty(newAvatar.AccountId))
            {
                return apiResponse.SetBadRequest(message: "AvatarRequest or AccountId is required");
            }

            try
            {
                var existingAccount = await unitOfWorks.AccountRepo.GetAsync(a => a.AccountId == newAvatar.AccountId);
                if (existingAccount == null)
                {
                    return apiResponse.SetNotFound("Account not found");
                }


                // Kiểm tra và cập nhật
                if (string.IsNullOrEmpty(newAvatar.Avatar))
                {
                    return apiResponse.SetBadRequest(message: "NewAvatar is required or cannot be empty");
                }

                await unitOfWorks.AccountRepo.UpdateFieldAsync(newAvatar.AccountId, a => a.Avatar!, newAvatar.Avatar);

                // Xác nhận giá trị sau cập nhật
                var updatedAccount = await unitOfWorks.AccountRepo.GetAsync(a => a.AccountId == newAvatar.AccountId);

                return apiResponse.SetOk("Avatar updated successfully");
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(message: ex.Message);
            }
        }
    }
}

// TOFIX
// Cannot call IRepos directly, need to go through unitofwork ... DONE FIXED

