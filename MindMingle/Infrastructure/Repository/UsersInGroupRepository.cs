using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class UsersInGroupRepository : GenericRepository<UsersInGroup>, IUsersInGroupRepository
    {
		public UsersInGroupRepository(MMDbContext mMDbContext) : base(mMDbContext)
		{
		}
	}
}

