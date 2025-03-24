using Application.Interface;
using Application.IRepository;
using Application.Request.Payment;
using Application.Response;
using Application.Response.Payment;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreatePaymentAsync(PaymentRequest paymentRequest)
        {
            try
            {
                if (paymentRequest == null)
                {
                    return new ApiResponse().SetBadRequest(message: "Payment cannot be null");
                }
                // Ánh xạ PaymentRequest sang Payment entity
                var payment = _mapper.Map<Payment>(paymentRequest);
                payment.PaymentMethod = PaymentMethod.MOMO;
                payment.PaymentStatus = PaymentStatus.PENDING;
                // Gọi AddAsync mà không gán vào biến, vì nó không trả về giá trị
                await _unitOfWorks.PaymentRepo.AddAsync(payment);

                // Nếu cần lấy lại payment (ví dụ để trả về ID), bạn có thể gọi GetAsync
                var createdPayment = await _unitOfWorks.PaymentRepo.GetAsync(p => p.PaymentId == payment.PaymentId);
                if (createdPayment == null)
                {
                    return new ApiResponse().SetApiResponse(
                        statusCode: HttpStatusCode.InternalServerError,
                        isSuccess: false,
                        message: "Failed to retrieve the created payment"
                    );
                }
                var paymentResponse = _mapper.Map<PaymentResponse>(createdPayment);
                return new ApiResponse().SetOk(createdPayment);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetApiResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    isSuccess: false,
                    message: $"Error creating payment: {ex.Message}"
                );
            }
        }

        public Task DeletePaymentAsync(string paymentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> GetPaymentByIdAsync(string paymentId)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentId))
                {
                    return new ApiResponse().SetBadRequest(message: "PaymentId cannot be null or empty");
                }

                var payment = await _unitOfWorks.PaymentRepo.GetPaymentWithDetailsAsync(paymentId);
                if (payment == null)
                {
                    return new ApiResponse().SetNotFound(message: $"Payment with ID {paymentId} not found");
                }
                var paymentResponse = _mapper.Map<PaymentResponse>(payment);

                return new ApiResponse().SetOk(payment);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetApiResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    isSuccess: false,
                    message: $"Error retrieving payment: {ex.Message}"
                );
            }
        }

        public Task<IEnumerable<ApiResponse>> GetPaymentsByPatientIdAsync(string patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> UpdateToPaidAsync(string paymentId)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentId))
                {
                    return new ApiResponse().SetBadRequest(message: "PaymentId cannot be null or empty");
                }

                var payment = await _unitOfWorks.PaymentRepo.GetAsync(x => x.PaymentId == paymentId);
                if (payment == null)
                {
                    return new ApiResponse().SetNotFound(message: $"Payment with ID {paymentId} not found");
                }
                // Kiểm tra trạng thái hiện tại (nếu cần)
                if (payment.PaymentStatus == PaymentStatus.PAID)
                {
                    return new ApiResponse().SetBadRequest(message: "Payment is already in PAID status");
                }

                await _unitOfWorks.PaymentRepo.UpdateFieldAsync(paymentId, x => x.PaymentStatus, PaymentStatus.PAID);

                return new ApiResponse().SetOk("Paid Successfully!");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetApiResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    isSuccess: false,
                    message: $"Error updating payment to PAID: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponse> UpdateToCanceledAsync(string paymentId)
        {
            try
            {
                if (string.IsNullOrEmpty(paymentId))
                {
                    return new ApiResponse().SetBadRequest(message: "PaymentId cannot be null or empty");
                }

                var payment = await _unitOfWorks.PaymentRepo.GetAsync(x => x.PaymentId == paymentId);
                if (payment == null)
                {
                    return new ApiResponse().SetNotFound(message: $"Payment with ID {paymentId} not found");
                }

                // Kiểm tra trạng thái hiện tại (nếu cần)
                if (payment.PaymentStatus == PaymentStatus.CANCELED)
                {
                    return new ApiResponse().SetBadRequest(message: "Payment is already in CANCELED status");
                }

                await _unitOfWorks.PaymentRepo.UpdateFieldAsync(paymentId, x => x.PaymentStatus, PaymentStatus.CANCELED);

                return new ApiResponse().SetOk("Canceled Successfully!");
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetApiResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    isSuccess: false,
                    message: $"Error updating payment to CANCELED: {ex.Message}"
                );
            }
        }
    }
}
