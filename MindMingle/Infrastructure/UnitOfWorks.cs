using System;
using Application;
using Application.IRepository;
using Domain.Entity;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class UnitOfWorks : IUnitOfWorks
	{
        private MMDbContext _mMDbContext;
        public IAccountRepository AccountRepo { get; }
        public IRoleRepository RoleRepo { get; }
        public ISignalRRepository SignalR { get; }

        public UnitOfWorks(MMDbContext mMDbContext)
        {
            _mMDbContext = mMDbContext;
            AccountRepo = new AccountRepository(mMDbContext);
            RoleRepo = new RoleRespository(mMDbContext);
            SignalR = new SignalRRepository(mMDbContext);
        }

       
    }
}

