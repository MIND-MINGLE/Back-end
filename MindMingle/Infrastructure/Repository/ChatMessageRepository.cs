using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
		public ChatMessageRepository(MMDbContext mMDbContext) : base(mMDbContext)
		{
		}
	}
}

