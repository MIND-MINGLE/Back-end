using Application.Request.Payment;
using Application.Response;
using Application.Response.Payment;
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
        Task<ApiResponse> PayWithPayOS(PaymentRequest paymentRequest);
        Task<ApiResponse> PayWithPayOS(PaymentRequestAppointment paymentRequest);
        Task<ApiResponse> ConfirmPayment(string paymentId, bool success);
        Task<ApiResponse> CreatePaymentAsync(PaymentRequest paymentRequest);
        Task<ApiResponse> GetPaymentByIdAsync(string paymentId);
        Task<IEnumerable<ApiResponse>> GetPaymentsByPatientIdAsync(string patientId);
        Task<ApiResponse> UpdateToPaidAsync(string paymentId);
        Task<ApiResponse> UpdateToCanceledAsync(string paymentId);
        Task<ApiResponse> GetPaymentsByPendingStatus(int pageIndex = 1, int pageSize = 10);
        Task<ApiResponse> GetAllPayments(int pageIndex = 1, int pageSize = 10);
        Task DeletePaymentAsync(string paymentId);
        Task<ApiResponse> GetPaymentHasAppointmentByPatientId(string patientId);
        Task<ApiResponse> CreatePaymentHasAppointmentAsync(PaymentRequestAppointment paymentRequest);

    }
}
