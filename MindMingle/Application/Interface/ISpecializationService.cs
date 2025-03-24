using Application.Request.Specialization;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ISpecializationService
    {
        Task<ApiResponse> CreateSpecializationAsync(SpecializationRequest specialization);
        Task<ApiResponse> UpdateSpecializationAsync(string specId, SpecializationRequest specialization);
        Task<ApiResponse> DisableSpecializationAsync(string specId);
        Task<ApiResponse> GetSpecializationByIdAsync(string specId);
        Task<ApiResponse> GetAllSpecializationsAsync();
    }
}
