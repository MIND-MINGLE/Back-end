using Application.Request.PurchasedPackage;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPurchasedPackageService
    {
        Task<ApiResponse> AddPurchasedPackageAsync(PurchasedPackageRequest purchasedPackageRequest);
        Task<ApiResponse> GetPurchasedPackageAsync();
        Task<ApiResponse> UpdatePurchasedStatus(string id);
        Task<ApiResponse> GetPurchasedPackageByPatientIdAsync(string patientId);
    }
}
