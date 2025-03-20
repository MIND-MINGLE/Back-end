using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class SubscriptionRepository: GenericRepository<Subscription>, ISubscriptionRepository
    {
		public SubscriptionRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
		}
	}
}

