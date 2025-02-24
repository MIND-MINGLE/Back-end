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
		public IEmailVerificationRepository EmailVerificationRepo { get; }
		public IPatientRepository PatientRepo { get; }
		public IChatGroupRepository ChatGroupRepo { get; }
		public IChatMessageRepository ChatMessageRepo { get; }
		public IUsersInGroupRepository UsersInGroupRepo { get; }

        public UnitOfWorks(MMDbContext mMDbContext, IOptions<TwilioOptions> options)
        {
            _mMDbContext = mMDbContext;
			EmailVerificationRepo = new EmailVerificationRepository(mMDbContext);
			AccountRepo = new AccountRepository(mMDbContext);
            RoleRepo = new RoleRespository(mMDbContext);
            TwilioRepo = new TwilioRepository(options);
			PatientRepo = new PatientRepository(mMDbContext);
			ChatGroupRepo = new ChatGroupRepository(mMDbContext);
			ChatMessageRepo = new ChatMessageRepository(mMDbContext);
			UsersInGroupRepo = new UsersInGroupRepository(mMDbContext);

        }
		public async Task SaveChangeAsync()
		{
			try
			{
				await _mMDbContext.SaveChangesAsync();

			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}


	}
}

