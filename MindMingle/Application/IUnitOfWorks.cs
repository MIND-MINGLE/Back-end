using System;
using Application.IRepository;

namespace Application
{
	public interface IUnitOfWorks
	{
        public IAccountRepository AccountRepo { get; }
        public IRoleRepository RoleRepo { get; }

        public ITwilioRepository TwilioRepo { get; }
        public IEmailVerificationRepository EmailVerificationRepo { get; }
        public IPatientRepository PatientRepo { get; }
        public ITherapistRepository TherapistRepo { get; }
        public IChatGroupRepository ChatGroupRepo { get; }
        public IChatMessageRepository ChatMessageRepo { get; }
        public IUsersInGroupRepository UsersInGroupRepo { get; }
        public IQuestionRepository QuestionRepo { get; }
        public ICategoryRepository CategoryRepo { get; }
        public IAnswerRepository AnswerRepo { get; }
        public ISessionRepository SessionRepo { get; }

        //TODO

        //public Task<T> ExecuteScalarAsync<T>(string sql);
        //public Task ExecuteRawSqlAsync(string sql);
        public Task SaveChangeAsync();

	}
}

