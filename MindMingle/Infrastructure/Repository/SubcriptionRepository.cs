using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class SubcriptionRepository: GenericRepository<Subcription>, ISubcriptionRepository
    {
		public SubcriptionRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
		}
	}
}

