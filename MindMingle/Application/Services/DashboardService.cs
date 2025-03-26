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

        public async Task<ApiResponse> GetDashboardStatsAsync()
        {
            try
            {
                // Lấy tổng doanh thu từ PaymentService
                var totalRevenue = await CalculateTotalRevenue();

                // Lấy tổng số bệnh nhân từ PatientService
                var totalPatients = await _patientService.GetTotalPatientsAsync();

                // Lấy tổng số nhà trị liệu từ TherapistService
                var totalTherapists = await _therapistService.GetTotalTherapistsAsync();

                // Lấy tổng số cuộc hẹn từ AppointmentService
                var totalAppointments = await _appointmentService.GetTotalAppointmentsAsync();

                // Lấy doanh thu theo tháng từ PaymentService
                var revenueByMonth = await CalculateRevenueByMonth();

                // Tạo response
                var stats = new DashboardStatsResponse
                {
                    TotalRevenue = totalRevenue,
                    TotalPatients = totalPatients,
                    TotalTherapists = totalTherapists,
                    TotalAppointments = totalAppointments,
                    RevenueByMonth = revenueByMonth
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
            // Giả định rằng PaymentService có phương thức GetAllPayments
            var paymentsResponse = await _paymentService.GetAllPayments();
            if (!paymentsResponse.IsSuccess)
            {
                return 0;
            }

            var payments = (paymentsResponse.Result as PagedResponse<PaymentResponse>)?.Items;
            if (payments == null)
            {
                return 0;
            }

            // Tính tổng doanh thu từ các Payment có trạng thái PAID
            return (decimal)payments
                .Where(p => p.PaymentStatus == PaymentStatus.PAID)
                .Sum(p => p.Amount);
        }

        private async Task<List<RevenueByMonthResponse>> CalculateRevenueByMonth()
        {
            // Giả định rằng PaymentService có phương thức GetAllPayments
            var paymentsResponse = await _paymentService.GetAllPayments();
            if (!paymentsResponse.IsSuccess)
            {
                return new List<RevenueByMonthResponse>();
            }

            var payments = (paymentsResponse.Result as PagedResponse<PaymentResponse>)?.Items;
            if (payments == null)
            {
                return new List<RevenueByMonthResponse>();
            }

            // Lấy danh sách Payment có trạng thái PAID
            var paidPayments = payments.Where(p => p.PaymentStatus == PaymentStatus.PAID).ToList();

            // Nhóm theo tháng và tính tổng doanh thu
            var revenueByMonth = paidPayments
                .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
                .Select(g => new RevenueByMonthResponse
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}", // Định dạng: "YYYY-MM"
                    Revenue = (decimal)g.Sum(p => p.Amount)
                })
                .OrderBy(r => r.Month)
                .ToList();

            return revenueByMonth;
        }
    }
}