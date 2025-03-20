using Application.Interface;
using Application.Request.Subcription;
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
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper _mapper;

        public SubscriptionService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            this.unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddSubscriptionAsync(SubscriptionRequest subRequest)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var subcription = _mapper.Map<Subcription>(subRequest);
                await unitOfWorks.SubcriptionRepo.AddAsync(subcription);
                await unitOfWorks.SaveChangeAsync();
                return response.SetOk(subRequest);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(null, ex.Message);
            }
        }

        public async Task<ApiResponse> DisableSubscriptionAsync(string id)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var subcription = await unitOfWorks.SubcriptionRepo.GetAsync(x => x.SubcriptionId == id);
                if (subcription == null)
                {
                    return response.SetNotFound("Subcription not found!");
                }
                subcription.IsDisabled = true;
                await unitOfWorks.SaveChangeAsync();
                return response.SetOk("Subcription disabled successfully");
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(null, ex.Message);
            }
        }

        public async Task<ApiResponse> GetSubscriptionAsync()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var subcriptions = await unitOfWorks.SubcriptionRepo.GetAllAsync(null);
                var resSubcriptions = _mapper.Map<List<ResponseSubcription>>(subcriptions);
                return response.SetOk(subcriptions);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(null, ex.Message);
            }
        }

        public async Task<ApiResponse> GetSubscriptionByIdAsync(string id)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var subcription = await unitOfWorks.SubcriptionRepo.GetAsync(x => x.SubcriptionId == id);
                if (subcription == null)
                {
                    return response.SetNotFound("Subcription not found!");
                }
                return response.SetOk(subcription);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(null, ex.Message);
            }
        }
    }
}
