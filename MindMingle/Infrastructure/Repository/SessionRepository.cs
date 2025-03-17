using System;
using Application.IRepository;
using Domain.Entity;

namespace Infrastructure.Repository
{
	public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
		public SessionRepository(MMDbContext context) : base(context)
        {
		}
	}
}

