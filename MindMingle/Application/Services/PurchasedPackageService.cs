using Application.Interface;
using Application.Request.PurchasedPackage;
using Application.Response;
using Application.Response.PurchasedPackage;
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
    public class PurchasedPackageService : IPurchasedPackageService
    {
        private readonly IUnitOfWorks _unitOfWork;
        private readonly IMapper _mapper;

        public PurchasedPackageService(IUnitOfWorks unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddPurchasedPackageAsync(PurchasedPackageRequest purchasedPackageRequest)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var purchasedPackage = _mapper.Map<PurchasedPackage>(purchasedPackageRequest);
                purchasedPackage.IsDisabled = true;
                await _unitOfWork.PurchasedPackageRepo.AddAsync(purchasedPackage);
                await _unitOfWork.SaveChangeAsync();
                return response.SetOk(purchasedPackageRequest);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetPurchasedPackageAsync()
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var purchasedPackages = await _unitOfWork.PurchasedPackageRepo.GetAllAsync(null, x => x.Include(p => p.Subscription));
                if (purchasedPackages.Count() == 0)
                {
                    return response.SetNotFound("Purchased packages not found!");
                }
                var resPurchasedPackages = _mapper.Map<List<ResponsePurchasedPackage>>(purchasedPackages);
                return response.SetOk(resPurchasedPackages);
            }
            catch (Exception ex)
            {
                return response.SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> GetPurchasedPackageByPatientIdAsync(string patientId)
        {
            try
            {
                ApiResponse response = new ApiResponse();
                var purchasedPackages = await _unitOfWork.PurchasedPackageRepo.GetAllAsync(x => x.PatientId == patientId, x => x.Include(p => p.Subscription));
                if (purchasedPackages == null)
                {
                    return response.SetNotFound("Purchased packages not found!");
                }
                var resPurchasedPackages = _mapper.Map<List<ResponsePurchasedPackage>>(purchasedPackages);
                return response.SetOk(resPurchasedPackages);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdatePurchasedStatus(string id)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var purchasedPackages = await _unitOfWork.PurchasedPackageRepo.GetAsync(x => x.PurchasedPackageId == id);
                if (purchasedPackages == null)
                {
                    return response.SetNotFound("Purchased package not found!");
                }
                await _unitOfWork.PurchasedPackageRepo.UpdateFieldAsync(purchasedPackages.PurchasedPackageId, p=>p.IsDisabled, purchasedPackages.IsDisabled=true);
                return response.SetOk("Status Update");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(ex.Message);
            }
        }
    }
}
