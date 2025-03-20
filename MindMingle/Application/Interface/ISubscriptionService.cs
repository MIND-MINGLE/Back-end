using Application.Request.Subcription;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ISubscriptionService
    {
        Task<ApiResponse> AddSubscriptionAsync(SubscriptionRequest subRequest);
        Task<ApiResponse> GetSubscriptionAsync();
        Task<ApiResponse> GetSubscriptionByIdAsync(string subcriptionId);
        Task<ApiResponse> DisableSubscriptionAsync(string subcriptionId);
    }
}
