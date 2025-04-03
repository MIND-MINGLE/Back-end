using Application.Request.Therapist_Specialization;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ITherapistSpecializationService
    {
        Task<ApiResponse> AddTherapistSpecializationAsync(TherapistSpecializationRequest request);
        Task<ApiResponse> GetTherapistSpecializationAsync();
        Task<ApiResponse> GetTherapistSpecializationByTherapistIdAsync(string therapistId);
        Task<ApiResponse> DeleteTherapistSpecializationByIdAsync(string therapistId, string specId);
    }
}
