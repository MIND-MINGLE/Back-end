using Application.Interface;
using Application.Request.Appointment;
using Application.Request.EmergencyEnd;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmergencyEndService : IEmergencyEndService
    {
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper mapper;
        private readonly IAppointmentService appointmentService;

        public EmergencyEndService(IUnitOfWorks unitOfWorks, IMapper mapper, IAppointmentService appointmentService)
        {
            this.unitOfWorks = unitOfWorks;
            this.mapper = mapper;
            this.appointmentService = appointmentService;
        }

        public async Task<ApiResponse> AddNewEmergencyEnd(EmergencyEndRequest newEmergencyEnd)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var emergencyEndModel = mapper.Map<EmergencyEnd>(newEmergencyEnd);
                emergencyEndModel.CreatedAt = DateTime.Now;
                emergencyEndModel.UpdatedAt = DateTime.Now;
                var appointmentModel = await unitOfWorks.AppointmentRepo.GetAsync(x => x.AppointmentId == newEmergencyEnd.AppointmentId);
                if (appointmentModel != null)
                {
                    AppointmentUpdateStatus appointmentUpdateStatus = new AppointmentUpdateStatus
                    {
                        Status = Status.CANCELED
                    };
                    var appointmentResponse = await appointmentService.UpdateAppointmentStatusAsync(newEmergencyEnd.AppointmentId, appointmentUpdateStatus);
                    if (appointmentResponse.IsSuccess)
                    {
                        await unitOfWorks.EmergencyEndRepo.AddAsync(emergencyEndModel);
                        await unitOfWorks.SaveChangeAsync();
                        return response.SetOk(newEmergencyEnd);
                    }
                    else
                    {
                        return response.SetNotFound("Cannot Update Appointment");
                    }
                }
                else
                {
                    return response.SetNotFound("No Appointment Found");
                }
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }
    }
}
