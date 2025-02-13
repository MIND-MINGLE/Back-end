using System;
using Domain.Entity;
using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class MMDbContext : DbContext
	{

     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<CoWorkingSpace> CoWorkingSpaces { get; set; }
		public DbSet<Credentials> Credentials { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UsersInGroup> UsersInGroups { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new TherapistConfig());
            modelBuilder.ApplyConfiguration(new CoWorkingSpaceConfig());
            modelBuilder.ApplyConfiguration(new CredentialsConfig());
            modelBuilder.ApplyConfiguration(new PatientConfig());
            modelBuilder.ApplyConfiguration(new ChatGroupConfig());
            modelBuilder.ApplyConfiguration(new ChatMessageConfig());
            modelBuilder.ApplyConfiguration(new UsersInGroupConfig());
            modelBuilder.ApplyConfiguration(new EmailVerificationConfig());

        }
        public MMDbContext(DbContextOptions<MMDbContext> options) : base(options)
        {

        }
    }
}

