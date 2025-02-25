using System;
using Application.Request.Patient;
using Application.Request.Therapist;
using Application.Response;

namespace Application.Interface
{
	public interface ITherapistService
	{
        Task<ApiResponse> AddNewTherapist(AddNewTherapistRequest newTherapist);
        Task<ApiResponse> GetTherapistByAccountIdAsync(string accountId);
    }
}

