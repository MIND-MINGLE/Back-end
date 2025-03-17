using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request.Account;
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
            var therapistAccount = await unitOfWorks.AccountRepo.GetAsync(x => x.AccountId == newTherapist.AccountId);
            if (therapistAccount == null)
            {
                response.SetBadRequest(message: "Account not found nor created!");
                return response;
            }

            //Create new therapist
            var therapist = mapper.Map<Therapist>(newTherapist);
            await unitOfWorks.TherapistRepo.AddAsync(therapist);
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

        public async Task<ApiResponse> UpdateTherapistAsync(UpdatePersonRequest updateTherapist)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var therapist = await unitOfWorks.TherapistRepo.GetAsync(x => x.TherapistId == updateTherapist.Id);
                if (therapist == null)
                {
                    response.SetBadRequest("Therapist profile not found.");
                    return response;
                }

                mapper.Map(updateTherapist, therapist);
                await unitOfWorks.TherapistRepo.UpdateFieldsAsync(therapist.TherapistId, new Dictionary<string, object>
            {
                { nameof(therapist.FirstName), therapist.FirstName },
                { nameof(therapist.LastName), therapist.LastName },
                { nameof(therapist.Dob), therapist.Dob },
                { nameof(therapist.Gender), therapist.Gender },
                { nameof(therapist.PhoneNumber), therapist.PhoneNumber }
            });
                await unitOfWorks.SaveChangeAsync();
                return response.SetOk(updateTherapist);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }

        public async Task<ApiResponse> GetAllTherapistAsync()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var therapistList = await unitOfWorks.TherapistRepo.GetAllAsync(null);
                var therapistResponseList = new List<ResponseTherapist>();
                foreach (Therapist therapist in therapistList)
                {
                    var formattedDob = therapist.Dob.Date.ToString("dd/MM/yyyy");
                    var therapistResponse = mapper.Map<ResponseTherapist>(therapist);
                    therapistResponse.Dob = formattedDob;
                    therapistResponseList.Add(therapistResponse);
                }
                return response.SetOk(therapistResponseList);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex);
            }
        }
    }   
}
