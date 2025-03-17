using Application.Interface;
using Application.IRepository;
using Application.Request.Appointment;
using Application.Response;
using Application.Response.Appointment;
using AutoMapper;
using Domain.Entity;
using System;
using System.Threading.Tasks;

namespace Application.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateAppointmentAsync(AppointmentRequest request)
        {
            if (request == null)
                return new ApiResponse().SetBadRequest(message: "Request data is null");

            try
            {
                var appointment = _mapper.Map<Appointment>(request);
                appointment.AppointmentId = Guid.NewGuid().ToString(); // Tự sinh Guid cho AppointmentId
                appointment.CreatedAt = DateTime.UtcNow;

                await _appointmentRepository.AddAsync(appointment);
                var response = _mapper.Map<AppointmentResponse>(appointment);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error creating appointment: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetAppointmentByIdAsync(string appointmentId)
        {
            if (string.IsNullOrEmpty(appointmentId))
                return new ApiResponse().SetBadRequest(message: "AppointmentId is required");

            try
            {
                var appointment = await _appointmentRepository.GetAsync(a => a.AppointmentId == appointmentId);
                if (appointment == null)
                    return new ApiResponse().SetNotFound("Appointment not found");

                var response = _mapper.Map<AppointmentResponse>(appointment);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching appointment: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetAppointmentsByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(patientId))
                return new ApiResponse().SetBadRequest(message: "PatientId is required");

            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByPatientIdAsync(patientId, pageIndex, pageSize);
                var response = _mapper.Map<List<AppointmentResponse>>(appointments);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching appointments: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetAppointmentsByTherapistIdAsync(string therapistId, int pageIndex = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(therapistId))
                return new ApiResponse().SetBadRequest(message: "TherapistId is required");

            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByTherapistIdAsync(therapistId, pageIndex, pageSize);
                var response = _mapper.Map<List<AppointmentResponse>>(appointments);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching appointments: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateAppointmentAsync(string appointmentId, AppointmentUpdateRequest request)
        {
            if (string.IsNullOrEmpty(appointmentId) || request == null)
                return new ApiResponse().SetBadRequest(message: "AppointmentId or request data is required");

            try
            {
                var appointment = await _appointmentRepository.GetAsync(a => a.AppointmentId == appointmentId);
                if (appointment == null)
                    return new ApiResponse().SetNotFound("Appointment not found");

                // Cập nhật các field nếu có trong request
                if (request.CoWorkingSpaceId != null)
                    await _appointmentRepository.UpdateFieldAsync(appointmentId, a => a.CoWorkingSpaceId, request.CoWorkingSpaceId);
                if (request.SessionId != null)
                    await _appointmentRepository.UpdateFieldAsync(appointmentId, a => a.SessionId, request.SessionId);
                if (request.EmergencyEndId != null)
                    await _appointmentRepository.UpdateFieldAsync(appointmentId, a => a.EmergencyEndId, request.EmergencyEndId);
                if (request.AppointmentType.HasValue)
                    await _appointmentRepository.UpdateFieldAsync(appointmentId, a => a.AppointmentType, request.AppointmentType.Value);
                if (request.Status.HasValue)
                    await _appointmentRepository.UpdateFieldAsync(appointmentId, a => a.Status, request.Status.Value);
                if (request.TotalFee.HasValue)
                    await _appointmentRepository.UpdateFieldAsync(appointmentId, a => a.TotalFee, request.TotalFee.Value);
                if (request.PlatformFee.HasValue)
                    await _appointmentRepository.UpdateFieldAsync(appointmentId, a => a.PlatformFee, request.PlatformFee.Value);

                // Lấy lại entity đã cập nhật để trả về
                var updatedAppointment = await _appointmentRepository.GetAsync(a => a.AppointmentId == appointmentId);
                var response = _mapper.Map<AppointmentResponse>(updatedAppointment);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error updating appointment: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteAppointmentAsync(string appointmentId)
        {
            if (string.IsNullOrEmpty(appointmentId))
                return new ApiResponse().SetBadRequest(message: "AppointmentId is required");

            try
            {
                var appointment = await _appointmentRepository.RemoveByIdAsync(appointmentId);
                var response = _mapper.Map<AppointmentResponse>(appointment);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error deleting appointment: {ex.Message}");
            }
        }
    }
}