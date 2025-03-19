using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class PurchasedPackageRepository: GenericRepository<PurchasedPackage>, IPurchasedPackageRepository
    {
		public PurchasedPackageRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
		}
	}
}

