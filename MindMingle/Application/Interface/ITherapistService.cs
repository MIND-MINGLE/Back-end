using System;
using Application.Request.Account;
using Application.Request.Patient;
using Application.Request.Therapist;
using Application.Response;

namespace Application.Interface
{
	public interface ITherapistService
	{
        Task<ApiResponse> AddNewTherapist(AddNewTherapistRequest newTherapist);
        Task<ApiResponse> GetTherapistByAccountIdAsync(string accountId);
        Task<ApiResponse> GetTherapistByTherapistIdAsync(string therapistId);
        Task<ApiResponse> UpdateTherapistAsync(UpdatePersonRequest request);
        Task<ApiResponse> GetAllTherapistAsync( );
        
    }
}

