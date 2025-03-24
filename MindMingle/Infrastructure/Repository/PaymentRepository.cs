using System;
using Application.IRepository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
	public class PaymentRepository: GenericRepository<Payment>, IPaymentRepository
    {
		private readonly MMDbContext _mMDbContext;
		public PaymentRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
			_mMDbContext = mMDbContext;
		}

        public new async Task AddAsync(Payment entity)
        {
            if (entity != null)
            {
                if (entity.PaymentMethod == default)
                {
                    entity.PaymentMethod = PaymentMethod.MOMO;
                }
                await _mMDbContext.Payments.AddAsync(entity);
                await _mMDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Payment entity cannot be null");
            }
        }

        public Task<Payment> GetPaymentByAppointmentIdAsync(string appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Payment>> GetPaymentsByPatientIdAsync(string patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> GetPaymentWithDetailsAsync(string paymentId)
        {
            return await GetAsync(
                p => p.PaymentId == paymentId,
                query => query
                    .Include(p => p.Patient)
                    .Include(p => p.Appointment)
            );
        }
    }
}

