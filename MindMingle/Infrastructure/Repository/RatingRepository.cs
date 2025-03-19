using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class RatingRepository: GenericRepository<Rating>, IRatingRepository
    {
		public RatingRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
		}
	}
}

