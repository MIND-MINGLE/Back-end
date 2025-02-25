using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request.Therapist;
using Application.Response;
using AutoMapper;
using Domain.Entity;

namespace Application.Services
{
    public class TherapistService : ITherapistService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWorks unitOfWorks;

        public TherapistService(IMapper mapper,IUnitOfWorks unitOfWorks)
        {
            this.mapper = mapper;
            this.unitOfWorks = unitOfWorks;
        }
        public async Task<ApiResponse> AddNewTherapist(AddNewTherapistRequest newTherapist)
        {
            ApiResponse response = new ApiResponse();

            //Check if account was create
            var patientAccount = await unitOfWorks.PatientRepo.GetAsync(x => x.AccountId == newTherapist.AccountId);
            if (patientAccount == null)
            {
                response.SetBadRequest(message: "Account not found nor created!");
                return response;
            }

            //Create new patient
            var patient = mapper.Map<Patient>(newTherapist);
            await unitOfWorks.PatientRepo.AddAsync(patient);
            await unitOfWorks.SaveChangeAsync();
            response.SetOk(newTherapist);
            //Console.WriteLine("Fixing Bug");
            return response;
        }

        public async Task<ApiResponse> GetTherapistByAccountIdAsync(string accountId)
        {
            ApiResponse response = new ApiResponse();
            var therapistModel = await unitOfWorks.TherapistRepo.GetAsync(t=>t.AccountId== accountId) ;
            if (therapistModel == null)
            {
                return response.SetNotFound(accountId);
            }
            else
            {
                var formattedDob = therapistModel.Dob.Date.ToString("dd/MM/yyyy");
                var therapistResponse = mapper.Map<ResponseTherapist>(therapistModel);
                therapistResponse.Dob = formattedDob;
            }

            return response.SetOk(therapistModel);
        }
    }   
}
