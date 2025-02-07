using System;
using Application;
using Application.IRepository;
using Application.Library;
using Domain.Entity;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
	public class UnitOfWorks : IUnitOfWorks
	{
        private MMDbContext _mMDbContext;
        public IAccountRepository AccountRepo { get; }
        public IRoleRepository RoleRepo { get; }
        public ITwilioRepository TwilioRepo { get; }

        public UnitOfWorks(MMDbContext mMDbContext, IOptions<TwilioOptions> options)
        {
            _mMDbContext = mMDbContext;
            AccountRepo = new AccountRepository(mMDbContext);
            RoleRepo = new RoleRespository(mMDbContext);
            TwilioRepo = new TwilioRepository(options);
        }

       
    }
}

