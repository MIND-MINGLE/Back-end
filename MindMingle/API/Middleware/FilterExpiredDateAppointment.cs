using Application;
using Domain.Entity;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class FilterExpiredDateAppointment
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FilterExpiredDateAppointment> _logger;

        public FilterExpiredDateAppointment(RequestDelegate next, ILogger<FilterExpiredDateAppointment> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            try
            {
                if (context == null || context.Request == null || string.IsNullOrEmpty(context.Request.Path.Value))
                {
                    _logger.LogWarning("HttpContext hoặc Request Path không hợp lệ.");
                    await _next(context);
                    return;
                }

                if (context.Request.Method == "GET" && context.Request.Path.Value.Contains("/therapist", StringComparison.OrdinalIgnoreCase))
                {
                    using (var scope = serviceProvider?.CreateScope())
                    {
                        if (scope == null)
                        {
                            _logger.LogError("Không thể tạo scope từ IServiceProvider.");
                            await _next(context);
                            return;
                        }

                        var unitOfWorks = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
                        if (unitOfWorks == null || unitOfWorks.AppointmentRepo == null)
                        {
                            _logger.LogError("IUnitOfWorks hoặc AppointmentRepo không được khởi tạo.");
                            await _next(context);
                            return;
                        }

                        var therapistId = context.Request.Path.Value.Split("/").LastOrDefault();
                        if (string.IsNullOrEmpty(therapistId))
                        {
                            _logger.LogWarning("TherapistId không hợp lệ hoặc không tìm thấy trong path.");
                            await _next(context);
                            return;
                        }

                        _logger.LogInformation($"TherapistId từ URL: {therapistId}");

                        // Fetch appointments
                        var appointments = await unitOfWorks.AppointmentRepo.GetAllAsync(
                            a => a.TherapistId == therapistId &&
                                 (a.Status == Status.PENDING || a.Status == Status.APPROVED),
                            s => s.Include(a => a.Session));
                        if (appointments == null || !appointments.Any())
                        {
                            _logger.LogWarning($"Không tìm thấy appointment nào cho TherapistId: {therapistId}.");
                            await _next(context);
                            return;
                        }

                        // Log danh sách appointments để kiểm tra
                        foreach (var appt in appointments)
                        {
                            _logger.LogInformation($"Appointment: Id={appt.AppointmentId}, Status={appt.Status}, EndTime={appt.Session?.EndTime:dd/MM/yyyy HH:mm:ss}");
                        }

                        // Sử dụng local time để so sánh
                        var currentTime = DateTime.Now; // Local time (UTC+7)
                        _logger.LogInformation($"Thời gian hiện tại (Local): {currentTime:dd/MM/yyyy HH:mm:ss}");

                        // Kiểm tra expired appointments
                        var expiredAppointments = appointments
                            .Where(a =>
                            {
                                if (a?.Session?.EndTime == null)
                                {
                                    _logger.LogWarning($"Appointment {a?.AppointmentId} có Session hoặc EndTime null.");
                                    return false;
                                }

                                var isExpired = a.Session.EndTime < currentTime;
                                _logger.LogInformation($"Appointment {a.AppointmentId}: EndTime={a.Session.EndTime:dd/MM/yyyy HH:mm:ss}, CurrentTime={currentTime:dd/MM/yyyy HH:mm:ss}, IsExpired={isExpired}");
                                return isExpired;
                            })
                            .ToList();

                        if (expiredAppointments.Any())
                        {
                            _logger.LogInformation($"Tìm thấy {expiredAppointments.Count} appointment hết hạn cho TherapistId: {therapistId}");
                            foreach (var appointment in expiredAppointments)
                            {
                                if (appointment == null || appointment.AppointmentId == null)
                                {
                                    _logger.LogWarning("Appointment hoặc AppointmentId không hợp lệ, bỏ qua cập nhật.");
                                    continue;
                                }

                                // Chuyển AppointmentId thành string
                                var appointmentIdStr = appointment.AppointmentId.ToString();

                                // Kiểm tra lại entity trước khi cập nhật
                                Guid.TryParse(appointmentIdStr, out var appointmentIdGuid);
                                var entity = await unitOfWorks.AppointmentRepo.GetAsync(a => a.AppointmentId == appointmentIdGuid.ToString());
                                if (entity == null)
                                {
                                    _logger.LogWarning($"Không tìm thấy appointment với ID {appointmentIdStr} trong database.");
                                    continue;
                                }

                                _logger.LogInformation($"Cập nhật appointment {appointmentIdStr} từ {appointment.Status} thành OVERDUE. EndTime: {appointment.Session.EndTime:dd/MM/yyyy HH:mm:ss}");

                                // Cập nhật trạng thái
                                await unitOfWorks.AppointmentRepo.UpdateFieldAsync(appointmentIdStr, a => a.Status, Status.OVERDUE);
                            }

                            // Lưu thay đổi vào database
                            _logger.LogInformation("Đã lưu thay đổi trạng thái appointment vào database.");
                        }
                        else
                        {
                            _logger.LogInformation($"Không có appointment nào hết hạn cho TherapistId: {therapistId}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi trong FilterExpiredDateAppointment middleware: {Message}", ex.Message);
            }

            await _next(context);
        }
    }
}