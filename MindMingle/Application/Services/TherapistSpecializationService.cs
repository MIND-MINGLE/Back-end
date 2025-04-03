using Application.Interface;
using Application.Request.Therapist_Specialization;
using Application.Response;
using Application.Response.Specialization;
using Application.Response.TherapistSpecialization;
using AutoMapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TherapistSpecializationService : ITherapistSpecializationService
    {
        private IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public TherapistSpecializationService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddTherapistSpecializationAsync(TherapistSpecializationRequest request)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var therapistSpecialization = _mapper.Map<Therapist_Specialization>(request);
                await _unitOfWorks.TherapistSpecializationRepo.AddAsync(therapistSpecialization);
                await _unitOfWorks.SaveChangeAsync();
                response.SetOk(request);
                return response;
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteTherapistSpecializationByIdAsync(string therapistId, string specId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var therapistSpecialization = await _unitOfWorks.TherapistSpecializationRepo.GetAsync(x => x.TherapistId == therapistId && x.SpecializationId == specId);
                if (therapistSpecialization == null)
                {
                    return response.SetNotFound("Therapist specialization not found.");
                }

                await _unitOfWorks.TherapistSpecializationRepo.RemoveByIdAsync(therapistSpecialization.Therapist_SpecializationId);
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk("Therapist specialization deleted successfully.");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetTherapistSpecializationAsync()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var therapistSpecialization = await _unitOfWorks.TherapistSpecializationRepo.GetAllAsync(null);
                var therapistSpecializationResponse = _mapper.Map<List<ResponseTherapistSpecialization>>(therapistSpecialization);
                if (therapistSpecializationResponse.Count == 0)
                {
                    return response.SetNotFound("No therapist specialization found.");
                }
                return response.SetOk(therapistSpecializationResponse);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetTherapistSpecializationByTherapistIdAsync(string therapistId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var therapistSpecializations = await _unitOfWorks.TherapistSpecializationRepo.GetAllAsync(
                    x => x.TherapistId == therapistId,
                    x => x.Include(p => p.Therapist).Include(p => p.Specialization));
               
                var firstTherapist = therapistSpecializations.First().Therapist;
                var responseSpecializations = _mapper.Map<List<ResponseSpecialization>>(
                    //using LINQ
                    therapistSpecializations.Select(ts => ts.Specialization).ToList()
                    );

                var therapistSpecializationResponse = new ResponseDetailTherapistSpecialization
                {
                    TherapistId = firstTherapist.TherapistId,
                    FirstName = firstTherapist.FirstName,
                    LastName = firstTherapist.LastName,
                    PhoneNumber = firstTherapist.PhoneNumber,
                    Description = firstTherapist.Description,
                    Dob = firstTherapist.Dob,
                    Gender = firstTherapist.Gender,
                    PricePerHour = firstTherapist.PricePerHour,
                    Specializations = responseSpecializations
                };

                return response.SetOk(therapistSpecializationResponse);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
    }
}
