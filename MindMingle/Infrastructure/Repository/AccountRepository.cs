using System;
using System.Linq.Expressions;
using Application.IRepository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(MMDbContext mMDbContext, DbSet<Account> db) : base(mMDbContext, db)
        {
        }
    }

}

