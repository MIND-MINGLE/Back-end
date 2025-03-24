using Application.Request.Payment;
using Application.Response;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPaymentService
    {
        Task<ApiResponse> CreatePaymentAsync(PaymentRequest paymentRequest);
        Task<ApiResponse> GetPaymentByIdAsync(string paymentId);
        Task<IEnumerable<ApiResponse>> GetPaymentsByPatientIdAsync(string patientId);
        Task<ApiResponse> UpdateToPaidAsync(string paymentId);
        Task<ApiResponse> UpdateToCanceledAsync(string paymentId);
        Task DeletePaymentAsync(string paymentId);
    }
}
