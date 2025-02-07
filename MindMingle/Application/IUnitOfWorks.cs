using System;
using Application.IRepository;

namespace Application
{
	public interface IUnitOfWorks
	{
		public IAccountRepository AccountRepo { get; }
        public IRoleRepository RoleRepo { get; }
        //TODO

        //public Task<T> ExecuteScalarAsync<T>(string sql);
        //public Task ExecuteRawSqlAsync(string sql);

    }
}

