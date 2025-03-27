using Application.Interface;
using Application.Request.Appointment;
using Application.Response;
using Application.Response.Appointment;
using AutoMapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;


namespace Application.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper _mapper;

        public AppointmentService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            this.unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }
        public async Task<int> GetTotalAppointmentsAsync()
        {
            return await unitOfWorks.AppointmentRepo.CountAsync();
        }
        public async Task<ApiResponse> GetAllAppointment()
        {
            ApiResponse response = new ApiResponse();
            var appList = await unitOfWorks.AppointmentRepo.GetAllAsync(null,x=>
            x.Include(p=>p.Patient)
            .Include(t => t.Therapist)
            .Include(s=>s.Session)
            .Include(e=>e.EmergencyEnd)
            );
            if (appList.Count == 0)
            {
                return response.SetNotFound("No Appointment Found");
            }
            var appListRes = _mapper.Map<List<AllAppointmentResponse>>(appList);
            return response.SetOk(appListRes);
        }
        public async Task<ApiResponse> UpdateAppointmentStatusApproved(string appointmentId)
        {
            var appointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
            if (appointment == null)
                return new ApiResponse().SetNotFound("Appointment not found");
            else
            {
                await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, x => x.Status, appointment.Status = Status.APPROVED);
            }
            var response = _mapper.Map<AppointmentResponse>(appointment);
            return new ApiResponse().SetOk(response);
        }
        public async Task<ApiResponse> UpdateAppointmentStatusCanceled(string appointmentId)
        {
            var appointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
            if (appointment == null)
                return new ApiResponse().SetNotFound("Appointment not found");
            else
            {
                await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, x => x.Status, appointment.Status = Status.CANCELED);
            }
            var response = _mapper.Map<AppointmentResponse>(appointment);
            return new ApiResponse().SetOk(response);
        }
        public async Task<ApiResponse> UpdateAppointmentStatusDeclined(string appointmentId)
        {
            var appointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
            if (appointment == null)
                return new ApiResponse().SetNotFound("Appointment not found");
            else
            {
                await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, x => x.Status, appointment.Status = Status.DECLINED);
            }
            var response = _mapper.Map<AppointmentResponse>(appointment);
            return new ApiResponse().SetOk(response);
        }
        public async Task<ApiResponse> CreateAppointmentAsync(AppointmentRequest request)
        {
            try
            {
                var appointment = _mapper.Map<Appointment>(request);
                appointment.AppointmentId = Guid.NewGuid().ToString(); // Tự sinh Guid cho AppointmentId
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.Status = Status.PENDING;
                await unitOfWorks.AppointmentRepo.AddAsync(appointment);
                await unitOfWorks.SaveChangeAsync();
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
                var appointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
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
                var appointments = await unitOfWorks.AppointmentRepo.GetAllAsync(
                    a => a.PatientId == patientId,
                    s => s.Include(a=>a.Session)
                    .Include(t=>t.Therapist)
                    );
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
                var appointments = await unitOfWorks.AppointmentRepo.GetAllAsync(
                    a => a.TherapistId == therapistId,
                    s=>s.Include(a=>a.Session).Include(p=>p.Patient)
                    );
                var response = _mapper.Map<List<AppointmentResponse>>(appointments);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching appointments: {ex.Message}");
            }
        }
        public async Task<ApiResponse> GetCurrentAppointments(string therapistId, string patientId)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (string.IsNullOrEmpty(therapistId))
                return apiResponse.SetBadRequest(message: "TherapistId is required");
            try
            {
                var appointments = await unitOfWorks.AppointmentRepo.GetAsync(a => a.TherapistId == therapistId && a.PatientId == patientId && !a.Status.Equals("Declined") && !a.Status.Equals("Canceled"),
                   s=>s.Include(a=>a.Session)
                );
                var response = _mapper.Map<AppointmentResponse>(appointments);
                return response != null ? apiResponse.SetOk(response) : apiResponse.SetNotFound("No Appoinment Found");
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
                var appointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
                if (appointment == null)
                    return new ApiResponse().SetNotFound("Appointment not found");

                // Cập nhật các field nếu có trong request
                if (request.CoWorkingSpaceId != null)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.CoWorkingSpaceId, request.CoWorkingSpaceId);
                if (request.SessionId != null)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.SessionId, request.SessionId);
                if (request.EmergencyEndId != null)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.EmergencyEndId, request.EmergencyEndId);
                if (request.AppointmentType.HasValue)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.AppointmentType, request.AppointmentType.Value);
                if (request.Status.HasValue)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.Status, request.Status.Value);
                if (request.TotalFee.HasValue)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.TotalFee, request.TotalFee.Value);
                if (request.PlatformFee.HasValue)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.PlatformFee, request.PlatformFee.Value);
                await unitOfWorks.SaveChangeAsync();
                // Lấy lại entity đã cập nhật để trả về
                var updatedAppointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
                var response = _mapper.Map<AppointmentResponse>(updatedAppointment);
                return new ApiResponse().SetOk(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error updating appointment: {ex.Message}");
            }
        }
        public async Task<ApiResponse> UpdateAppointmentStatusAsync(string appointmentId, AppointmentUpdateStatus request)
        {
            if (string.IsNullOrEmpty(appointmentId) || request == null)
                return new ApiResponse().SetBadRequest(message: "AppointmentId or request data is required");

            try
            {
                var appointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
                if (appointment == null)
                    return new ApiResponse().SetNotFound("Appointment not found");

                // Cập nhật các field nếu có trong request

                if (request.Status.HasValue)
                    await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentId, a => a.Status, request.Status.Value);
                await unitOfWorks.SaveChangeAsync();
                // Lấy lại entity đã cập nhật để trả về
                var updatedAppointment = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentId);
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
                var appointment = await unitOfWorks.AppointmentRepo.RemoveByIdAsync(appointmentId);
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