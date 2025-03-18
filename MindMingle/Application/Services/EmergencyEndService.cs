using Application.Interface;
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

        public EmergencyEndService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            this.unitOfWorks = unitOfWorks;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddNewEmergencyEnd(EmergencyEndRequest newEmergencyEnd)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var emergencyEndModel = mapper.Map<EmergencyEnd>(newEmergencyEnd);
                await unitOfWorks.EmergencyEndRepo.AddAsync(emergencyEndModel);
                await unitOfWorks.SaveChangeAsync();
                return response.SetOk(newEmergencyEnd);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }
    }
}
