using System;
using Domain.Entity;

namespace Application.IRepository
{
	public interface IPaymentRepository:IGenericRepository<Payment>
	{
        Task<Payment> GetPaymentByAppointmentIdAsync(string appointmentId);
        Task<IEnumerable<Payment>> GetPaymentsByPatientIdAsync(string patientId);
        Task<Payment> GetPaymentWithDetailsAsync(string paymentId);
    }
}

