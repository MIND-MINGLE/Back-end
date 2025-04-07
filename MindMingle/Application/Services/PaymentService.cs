using Application.Interface;
using Application.IRepository;
using Application.Request.Payment;
using Application.Response;
using Application.Response.Payment;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        
        public PaymentService(IUnitOfWorks unitOfWorks, IMapper mapper, IConfiguration configuration)
        {
            this.configuration = configuration;
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

        public async Task<ApiResponse> GetPaymentsByPendingStatus(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                // Lấy tổng số bản ghi trước khi phân trang
                var totalCount = await _unitOfWorks.PaymentRepo.CountAsync(x => x.PaymentStatus == PaymentStatus.PENDING);

                // Lấy danh sách với phân trang
                var payments = await _unitOfWorks.PaymentRepo.GetAllAsync(
                    x => x.PaymentStatus == PaymentStatus.PENDING,
                    x => x.Include(p => p.Patient).Include(a => a.Appointment),
                    pageIndex,
                    pageSize
                );

                if (payments == null || !payments.Any())
                {
                    return new ApiResponse().SetNotFound(message: "No payments with PENDING status found");
                }

                var response = _mapper.Map<IEnumerable<PaymentResponse>>(payments);
                var pagedResponse = new PagedResponse<PaymentResponse>(response, totalCount, pageIndex, pageSize);
                return new ApiResponse().SetOk(pagedResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetApiResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    isSuccess: false,
                    message: $"Error retrieving payments with PENDING status: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "No inner exception"}"
                );
            }
        }
        public async Task<ApiResponse> GetAllPayments(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                // Lấy tổng số bản ghi trước khi phân trang
                var totalCount = await _unitOfWorks.PaymentRepo.CountAsync();

                // Lấy danh sách với phân trang
                var payments = await _unitOfWorks.PaymentRepo.GetAllAsync(
                    null,
                    x => x.Include(p => p.Patient),
                    pageIndex,
                    pageSize
                );

                if (payments == null || !payments.Any())
                {
                    return new ApiResponse().SetNotFound(message: "No payments found");
                }

                var response = _mapper.Map<IEnumerable<PaymentResponse>>(payments);
                var pagedResponse = new PagedResponse<PaymentResponse>(response, totalCount, pageIndex, pageSize);
                return new ApiResponse().SetOk(pagedResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetApiResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    isSuccess: false,
                    message: $"Error retrieving payments: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "No inner exception"}"
                );
            }
        }

        public Task<IEnumerable<ApiResponse>> GetPaymentHasAppointmentByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> CreatePaymentHasAppointmentAsync(PaymentRequestAppointment paymentRequest)
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
        public async Task<ApiResponse> PayWithPayOS(PaymentRequest paymentRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            var payment = _mapper.Map<Payment>(paymentRequest);
            payment.PaymentId = Guid.NewGuid().ToString();
            payment.PaymentMethod = PaymentMethod.MOMO;
            payment.PaymentStatus = PaymentStatus.PENDING;
            await _unitOfWorks.PaymentRepo.AddAsync(payment);
            await _unitOfWorks.SaveChangeAsync();

            var clientId = configuration.GetSection("PayOS").GetSection("PayOSClientID").Value;
            var apiKey = configuration.GetSection("PayOS").GetSection("PayOSAPIKey").Value;
            var checksumKey = configuration.GetSection("PayOS").GetSection("PayOSChecksumKey").Value;
            var random = new Random(); // generate a order code
            // Redirect to here, the API to confirm payment
            var domain = "https://mindmingle202.azurewebsites.net/api/Payment/receiveTransaction";
            if(clientId==null|| apiKey==null||checksumKey==null){
                return apiResponse.SetBadRequest("Cannot create PayOS url");
            }
            var payOS = new PayOS(clientId, apiKey, checksumKey);
            List<ItemData> itemDatas = new List<ItemData>
            {
                new("Deposit", 1, (int)payment.Amount)
            };
            var paymentLinkRequest = new PaymentData(
                orderCode: random.Next(),
                amount: (int)payment.Amount,
                description: "Payment",
                items: itemDatas,
                returnUrl: domain + $"?paymentId={payment.PaymentId}&success=true",
                cancelUrl: domain + $"?paymentId={payment.PaymentId}&success=false"
            );
            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return apiResponse.SetOk(response.checkoutUrl);
        }
        public async Task<ApiResponse> PayWithPayOS(PaymentRequestAppointment paymentRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            var payment = _mapper.Map<Payment>(paymentRequest);
            payment.PaymentId = Guid.NewGuid().ToString();
            payment.PaymentMethod = PaymentMethod.MOMO;
            payment.PaymentStatus = PaymentStatus.PENDING;
            await _unitOfWorks.PaymentRepo.AddAsync(payment);
            await _unitOfWorks.SaveChangeAsync();

            var clientId = configuration.GetSection("PayOS").GetSection("PayOSClientID").Value;
            var apiKey = configuration.GetSection("PayOS").GetSection("PayOSAPIKey").Value;
            var checksumKey = configuration.GetSection("PayOS").GetSection("PayOSChecksumKey").Value;
            var random = new Random(); // generate a order code
            var domain = "https://mindmingleexe202.azurewebsites.net/api/Payment/receiveTransaction";
            if (clientId == null || apiKey == null || checksumKey == null)
            {
                return apiResponse.SetBadRequest("Cannot create PayOS url");
            }
            var payOS = new PayOS(clientId, apiKey, checksumKey);
            List<ItemData> itemDatas = new List<ItemData>
            {
                new("Deposit", 1, (int)payment.Amount)
            };
            var paymentLinkRequest = new PaymentData(
                orderCode: random.Next(),
                amount: (int)payment.Amount,
                description: "Payment",
                items: itemDatas,
                returnUrl: domain + $"?success=true&transactionId={payment.PaymentId}",
                cancelUrl: domain + $"?success=false&transactionId={payment.PaymentId}"
            );
            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return apiResponse.SetOk(response.checkoutUrl);
        }


        public async Task<ApiResponse> ConfirmPayment(string paymentId,bool success)
        {
            ApiResponse apiResponse = new ApiResponse();
            var payment = await _unitOfWorks.PaymentRepo.GetAsync(p=>p.PaymentId.Equals(paymentId));
                if (success)
                {
                    await _unitOfWorks.PaymentRepo.UpdateFieldAsync(paymentId,p=>p.PaymentStatus, PaymentStatus.PAID);
                // For Checking Subscription Data
                var now = DateTime.UtcNow; 
                // Fetch all matching subscriptions and sort by proximity to now
                var subscriptions = await _unitOfWorks.PurchasedPackageRepo
                    .GetAllAsync(pu => pu.PatientId == payment.PatientId && pu.IsDisabled == true);
                if (subscriptions.Count> 0)
                {
                    var closestSubscription = subscriptions
                    .OrderBy(pu => Math.Abs((pu.StartDate - now).TotalSeconds))
                    .FirstOrDefault();
                    await _unitOfWorks.PurchasedPackageRepo.UpdateFieldAsync(closestSubscription?.PurchasedPackageId, p => p.IsDisabled, false);
                }
                // For Checking Appointment Data TODO

                PayOSResponse payOSResponse = new PayOSResponse
                {
                    PaymentId = paymentId,
                    PaymentStatus = true
                };
                return apiResponse.SetOk(payOSResponse);
                }
                else
                {
                await _unitOfWorks.PaymentRepo.UpdateFieldAsync(paymentId, p => p.PaymentStatus, PaymentStatus.CANCELED);
                PayOSResponse payOSResponse = new PayOSResponse
                {
                    PaymentId = paymentId
                 ,
                    PaymentStatus = false
                };
                return apiResponse.SetOk(payOSResponse);
            }
                
        }
    }
}
