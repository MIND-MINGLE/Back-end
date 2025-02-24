using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class ChatGroupRepository : GenericRepository<ChatGroup>, IChatGroupRepository
    {
		public ChatGroupRepository(MMDbContext mMDbContext) : base(mMDbContext)
		{
		}
	}
}

