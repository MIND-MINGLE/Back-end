using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class PaymentRepository: GenericRepository<Payment>, IPaymentRepository
    {
		public PaymentRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
		}
	}
}

