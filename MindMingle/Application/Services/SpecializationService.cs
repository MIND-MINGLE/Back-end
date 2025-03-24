using Application.Interface;
using Application.IRepository;
using Application.Request.Specialization;
using Application.Response;
using Application.Response.Specialization;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public SpecializationService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateSpecializationAsync(SpecializationRequest specialization)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var spec = await _unitOfWorks.SpecializationRepo.GetAsync(x => x.Name.Equals(specialization.Name));
                if (spec != null)
                {
                    return response.SetBadRequest("Specialization already exist");
                }
                spec = _mapper.Map<Specialization>(specialization);
                await _unitOfWorks.SpecializationRepo.AddAsync(spec);
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk(spec);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> DisableSpecializationAsync(string specId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var spec = await _unitOfWorks.SpecializationRepo.GetAsync(x => x.SpecializationId == specId);
                if (spec == null)
                {
                    return response.SetNotFound("Specialization not found");
                }
                spec.IsDisabled = true;
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk("Specialization removed");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllSpecializationsAsync()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var specializations = await _unitOfWorks.SpecializationRepo.GetAllAsync(null);
                if (specializations.Count == 0)
                {
                    return response.SetNotFound("No specialization found");
                }
                else
                {
                    var resSpec = _mapper.Map<List<ResponseSpecialization>>(specializations);
                    return response.SetOk(resSpec);
                }
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSpecializationByIdAsync(string specId)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var spec = await _unitOfWorks.SpecializationRepo.GetAsync(x => x.SpecializationId == specId);
                if (spec == null)
                {
                    return response.SetNotFound("Specialization not found");
                }
                var resSpec = _mapper.Map<ResponseSpecialization>(spec);
                return response.SetOk(resSpec);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateSpecializationAsync(string specId, SpecializationRequest specialization)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var spec = await _unitOfWorks.SpecializationRepo.GetAsync(x => x.SpecializationId == specId);
                if (spec == null)
                {
                    return response.SetNotFound("Specialization not found");
                }
                spec.Name = specialization.Name;
                spec.Description = specialization.Description;
                spec.UpdatedAt = DateTime.UtcNow;
                await _unitOfWorks.SaveChangeAsync();
                return response.SetOk(spec);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }
    }
}
