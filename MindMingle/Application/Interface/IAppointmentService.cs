using Application.Request.Appointment;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAppointmentService
    {
        Task<int> GetTotalAppointmentsAsync();
        Task<ApiResponse> CreateAppointmentAsync(AppointmentRequest request);
        Task<ApiResponse> GetAllAppointment();
        Task<ApiResponse> GetAppointmentByIdAsync(string appointmentId);
        Task<ApiResponse> GetAppointmentsByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10);
        Task<ApiResponse> GetAppointmentsByTherapistIdAsync(string therapistId, int pageIndex = 1, int pageSize = 10);
        Task<ApiResponse> UpdateAppointmentAsync(string appointmentId, AppointmentUpdateRequest request);
        Task<ApiResponse> DeleteAppointmentAsync(string appointmentId);
        Task<ApiResponse> GetCurrentAppointments(string therapistId, string patientId);
        Task<ApiResponse> UpdateAppointmentStatusAsync(string appointmentId, AppointmentUpdateStatus request);
       
    }
}
