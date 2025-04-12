using System;
using Application.Request.Account;
using Application.Request.Patient;
using Application.Request.Therapist;
using Application.Response;

namespace Application.Interface
{
	public interface ITherapistService
	{
        Task<int> GetTotalTherapistsAsync();
        Task<ApiResponse> AddNewTherapist(AddNewTherapistRequest newTherapist);
        Task<ApiResponse> GetTherapistByAccountIdAsync(string accountId);
        Task<ApiResponse> GetTherapistByTherapistIdAsync(string therapistId);
        Task<ApiResponse> UpdateTherapistAsync(UpdateTherapistRequest request);
        Task<ApiResponse> GetAllTherapistAsync( );
        Task<ApiResponse> ApproveToBecomeTherapist(string therapistId);
        
    }
}

