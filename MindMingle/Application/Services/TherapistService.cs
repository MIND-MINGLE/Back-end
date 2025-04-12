
using Application.Interface;
using Application.Request.Account;
using Application.Request.Therapist;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        public async Task<int> GetTotalTherapistsAsync()
        {
            return await unitOfWorks.TherapistRepo.CountAsync();
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

        public async Task<ApiResponse> ApproveToBecomeTherapist(string accountId)
        {
            ApiResponse response = new ApiResponse();
            var account = await unitOfWorks.AccountRepo.GetAsync(x => x.AccountId == accountId);
            if (account == null)
            {
                return response.SetNotFound(accountId);
            }
            else
            {
                await unitOfWorks.AccountRepo.UpdateFieldAsync(accountId, a => a.IsDisabled,false);
                return response.SetOk("Update Completed!");
            }
        }
        public async Task<ApiResponse> GetTherapistByAccountIdAsync(string accountId)
        {
            ApiResponse response = new ApiResponse();
            var therapistModel = await unitOfWorks.TherapistRepo.GetAsync(
            t => t.AccountId == accountId,
            q => q.Include(t => t.Account) // Assuming 'Account' is the navigation property
        );
            if (therapistModel == null)
            {
                return response.SetNotFound(accountId);
            }
            else
            {
                var formattedDob = therapistModel.Dob.Date.ToString("dd/MM/yyyy");
                var therapistResponse = mapper.Map<ResponseTherapist>(therapistModel);
                therapistResponse.Dob = formattedDob;
                return response.SetOk(therapistResponse);
            }
        }

        public async Task<ApiResponse> GetTherapistByTherapistIdAsync(string therapistId)
        {
            ApiResponse response = new ApiResponse();
            var therapistModel = await unitOfWorks.TherapistRepo.GetAsync(
            t => t.TherapistId == therapistId,
            q => q.Include(t => t.Account) // Assuming 'Account' is the navigation property
        );
            if (therapistModel == null)
            {
                return response.SetNotFound(therapistId);
            }
            else
            {
                var formattedDob = therapistModel.Dob.Date.ToString("dd/MM/yyyy");
                var therapistResponse = mapper.Map<ResponseTherapist>(therapistModel);
                therapistResponse.Dob = formattedDob;
                return response.SetOk(therapistResponse);
            }
        }

        public async Task<ApiResponse> UpdateTherapistAsync(UpdateTherapistRequest updateTherapist)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var therapist = await unitOfWorks.TherapistRepo.GetAsync(x => x.TherapistId == updateTherapist.Id);
                if (therapist == null)
                {
                    return response.SetNotFound(null, "Therapist profile not found.");
                }

                // Chỉ cập nhật các trường không null
                var fieldsToUpdate = new Dictionary<string, object>();
                if (updateTherapist.FirstName != null) fieldsToUpdate.Add(nameof(therapist.FirstName), updateTherapist.FirstName);
                if (updateTherapist.LastName != null) fieldsToUpdate.Add(nameof(therapist.LastName), updateTherapist.LastName);
                if (updateTherapist.Dob.HasValue) fieldsToUpdate.Add(nameof(therapist.Dob), updateTherapist.Dob.Value);
                if (updateTherapist.Gender != null) fieldsToUpdate.Add(nameof(therapist.Gender), updateTherapist.Gender);
                if (updateTherapist.PhoneNumber != null) fieldsToUpdate.Add(nameof(therapist.PhoneNumber), updateTherapist.PhoneNumber);
                if (updateTherapist.Description != null) fieldsToUpdate.Add(nameof(therapist.Description), updateTherapist.Description);

                if (fieldsToUpdate.Any())
                {
                    await unitOfWorks.TherapistRepo.UpdateFieldsAsync(therapist.TherapistId, fieldsToUpdate);
                    await unitOfWorks.SaveChangeAsync();
                }

                return response.SetOk(therapist);
            }
            catch (DbUpdateException ex)
            {
                return response.SetApiResponse(HttpStatusCode.InternalServerError, false, "Database error: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(null, "An error occurred: " + ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllTherapistAsync()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var therapistList = await unitOfWorks.TherapistRepo.GetAllAsync(null,
                    x => x.Include(t => t.Account)
                    );
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
                string errorMessage = $"Error fetching therapists: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $". Details: {ex.InnerException.Message}";
                }
                return response.SetBadRequest(errorMessage);
            }
        }
    }   
}
