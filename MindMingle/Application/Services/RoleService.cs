using System;
using Application.Interface;
using Application.Response;
using AutoMapper;

namespace Application.Services
{
	public class RoleService : IRoleService
	{
        private readonly IMapper mapper;
        private readonly IUnitOfWorks unitOfWorks;


        public RoleService(IMapper mapper, IUnitOfWorks unitOfWorks)
        {
            this.mapper = mapper;
            this.unitOfWorks = unitOfWorks;
        }

        public async Task<ApiResponse> GetAllRoles()
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var roleModel = await unitOfWorks.RoleRepo.GetAllAsync(null);
                var resRole = mapper.Map<List<ResponseRole>>(roleModel); 
                Console.WriteLine($"Fetch data complete: {resRole.Count}");
                return apiResponse.SetOk(resRole);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }
    }
}

