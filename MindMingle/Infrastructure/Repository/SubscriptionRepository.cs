using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class SubscriptionRepository: GenericRepository<Subcription>, ISubscriptionRepository
    {
		public SubscriptionRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
		}
	}
}

