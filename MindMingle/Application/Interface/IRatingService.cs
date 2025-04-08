using Application.Request.Rating;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IRatingService
    {
        Task<ApiResponse> AddRatingAsync(RatingRequest request);
        Task<ApiResponse> GetAllRatingAsync();
        Task<ApiResponse> GetRatingByTherapistIdAsync(string therapistId);
        Task<ApiResponse> GetRatingByPatientIdAsync(string patientId);
        Task<ApiResponse> GetRatingByAppointmentIdAsync(string appointmentId);
        Task<ApiResponse> GetAverageRatingByTherapistIdAsync(string therapistId);
    }
}
