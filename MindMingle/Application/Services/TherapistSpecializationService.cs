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
                var therapistSpecialization = await _unitOfWorks.TherapistSpecializationRepo.GetAsync(
                    x => x.TherapistId == therapistId,
                    x => x.Include(p => p.Therapist).Include(p => p.Specialization)
                );
                if (therapistSpecialization == null)
                {
                    return response.SetNotFound("No therapist specialization found.");
                }
                var therapist = await _unitOfWorks.TherapistRepo.GetAsync(x => x.TherapistId == therapistId);
                var specialization = await _unitOfWorks.SpecializationRepo.GetAsync(x => x.SpecializationId == therapistSpecialization.SpecializationId);

                var therapistSpecializationResponse = new ResponseDetailTherapistSpecialization
                {
                    TherapistId = therapistSpecialization.TherapistId,
                    FirstName = therapist.FirstName,
                    LastName = therapist.LastName,
                    PhoneNumber = therapist.PhoneNumber,
                    Description = therapist.Description,
                    Dob = therapist.Dob,
                    Gender = therapist.Gender,
                    PricePerHour = therapist.PricePerHour,
                    Specializations = new List<ResponseSpecialization>
                    {
                        new ResponseSpecialization
                        {
                            SpecializationId = specialization.SpecializationId,
                            Name = specialization.Name,
                            Description = "",
                        }
                    }.ToList()
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
