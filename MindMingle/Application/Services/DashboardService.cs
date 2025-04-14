using Application.Interface;
using Application.Response.Payment;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Response.Dashboard;
using Domain.Entity;

namespace Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IPaymentService _paymentService;
        private readonly IPatientService _patientService;
        private readonly ITherapistService _therapistService;
        private readonly IAppointmentService _appointmentService;

        public DashboardService(
            IPaymentService paymentService,
            IPatientService patientService,
            ITherapistService therapistService,
            IAppointmentService appointmentService)
        {
            _paymentService = paymentService;
            _patientService = patientService;
            _therapistService = therapistService;
            _appointmentService = appointmentService;
        }

        // Thêm tham số timeRange vào phương thức
        public async Task<ApiResponse> GetDashboardStatsAsync(string timeRange = "month")
        {
            try
            {
                // Xác thực giá trị timeRange
                timeRange = timeRange.ToLower();
                if (!new[] { "week", "month", "year" }.Contains(timeRange))
                {
                    return new ApiResponse().SetApiResponse(
                        statusCode: HttpStatusCode.BadRequest,
                        isSuccess: false,
                        message: "Invalid timeRange. Allowed values: 'week', 'month', 'year'."
                    );
                }

                // Lấy tổng doanh thu từ PaymentService
                var totalRevenue = await CalculateTotalRevenue();

                // Lấy tổng số bệnh nhân từ PatientService
                var totalPatients = await _patientService.GetTotalPatientsAsync();

                // Lấy tổng số nhà trị liệu từ TherapistService
                var totalTherapists = await _therapistService.GetTotalTherapistsAsync();

                // Lấy tổng số cuộc hẹn từ AppointmentService
                var totalAppointments = await _appointmentService.GetTotalAppointmentsAsync();

                // Lấy doanh thu theo khoảng thời gian (tuần, tháng, năm)
                var revenueByTimeRange = await CalculateRevenueByTimeRange(timeRange);

                // Tạo response
                var stats = new DashboardStatsResponse
                {
                    TotalRevenue = totalRevenue,
                    TotalPatients = totalPatients,
                    TotalTherapists = totalTherapists,
                    TotalAppointments = totalAppointments,
                    RevenueByMonth = revenueByTimeRange // Tạm giữ tên field, có thể đổi nếu cần
                };

                return new ApiResponse().SetOk(stats);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetApiResponse(
                    statusCode: HttpStatusCode.InternalServerError,
                    isSuccess: false,
                    message: $"Error retrieving dashboard stats: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "No inner exception"}"
                );
            }
        }

        private async Task<decimal> CalculateTotalRevenue()
        {
            var paymentsResponse = await _paymentService.GetAllPaymentsWithoutPagination();
            if (!paymentsResponse.IsSuccess)
            {
                return 0;
            }

            var payments = paymentsResponse.Result as IEnumerable<PaymentResponse>;
            if (payments == null)
            {
                return 0;
            }

            return (decimal)payments
                .Where(p => p.PaymentStatus == PaymentStatus.PAID)
                .Sum(p => p.Amount);
        }

        // Phương thức mới thay thế CalculateRevenueByMonth
        private async Task<List<RevenueByMonthResponse>> CalculateRevenueByTimeRange(string timeRange)
        {
            var paymentsResponse = await _paymentService.GetAllPaymentsWithoutPagination();
            if (!paymentsResponse.IsSuccess)
            {
                return new List<RevenueByMonthResponse>();
            }


            var payments = paymentsResponse.Result as IEnumerable<PaymentResponse>;
            if (payments == null || !payments.Any())
            {
                return new List<RevenueByMonthResponse>();
            }

            var paidPayments = payments.Where(p => p.PaymentStatus == PaymentStatus.PAID).ToList();

            // Logic phân loại theo week, month, year
            List<RevenueByMonthResponse> revenueByTimeRange;

            switch (timeRange)
            {
                case "week":
                    revenueByTimeRange = paidPayments
                        .GroupBy(p => new
                        {
                            p.CreatedAt.Year,
                            Week = System.Globalization.ISOWeek.GetWeekOfYear(p.CreatedAt)
                        })
                        .Select(g => new RevenueByMonthResponse
                        {
                            Month = $"{g.Key.Year}-W{g.Key.Week:D2}", // Định dạng: "YYYY-Www"
                            Revenue = (decimal)g.Sum(p => p.Amount)
                        })
                        .OrderBy(r => r.Month)
                        .ToList();
                    break;

                case "month":
                    revenueByTimeRange = paidPayments
                        .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
                        .Select(g => new RevenueByMonthResponse
                        {
                            Month = $"{g.Key.Year}-{g.Key.Month:D2}", // Định dạng: "YYYY-MM"
                            Revenue = (decimal)g.Sum(p => p.Amount)
                        })
                        .OrderBy(r => r.Month)
                        .ToList();
                    break;

                case "year":
                    revenueByTimeRange = paidPayments
                        .GroupBy(p => p.CreatedAt.Year)
                        .Select(g => new RevenueByMonthResponse
                        {
                            Month = $"{g.Key}", // Định dạng: "YYYY"
                            Revenue = (decimal)g.Sum(p => p.Amount)
                        })
                        .OrderBy(r => r.Month)
                        .ToList();
                    break;

                default:
                    revenueByTimeRange = new List<RevenueByMonthResponse>();
                    break;
            }

            return revenueByTimeRange;
        }
    }
}