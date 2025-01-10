using System;
using Application.Response;

namespace Application.Interface
{
	public interface IRoleService
	{
        public Task<ApiResponse> GetAllRoles();

    }
}

