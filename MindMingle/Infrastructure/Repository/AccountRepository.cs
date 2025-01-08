using System;
using System.Linq.Expressions;
using Application.IRepository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repository
{
	public class AccountRepository : IAccountRepository,IGenericRepository<Account>
	{
		public AccountRepository()
		{
		}

        public Task AddAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(List<Account> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> GetAllAsync(Expression<Func<Account, bool>>? filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> GetAllAsync(Expression<Func<Account, bool>>? filter, Func<IQueryable<Account>, IIncludableQueryable<Account, object>>? include = null, int pageIndex = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAsync(Expression<Func<Account, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAsync(Expression<Func<Account, bool>> filter, Func<IQueryable<Account>, IIncludableQueryable<Account, object>>? include)
        {
            throw new NotImplementedException();
        }

        public Task<Account> RemoveByIdAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}

