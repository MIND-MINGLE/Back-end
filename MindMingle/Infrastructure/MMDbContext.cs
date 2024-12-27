using System;
using Domain.Entity;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
            .HasOne(c => c.Role)          
            .WithOne(a => a.Account)        
            .HasForeignKey<Account>(v => v.RoleId) 
            .OnDelete(DeleteBehavior.Cascade);
            // Configuring one-to-one relationship between Role and Account
            modelBuilder.Entity<Patient>()
           .HasOne(c => c.Account)
           .WithOne(a => a.Patient)
           .HasForeignKey<Patient>(v => v.AccountId)
           .OnDelete(DeleteBehavior.Cascade);

           modelBuilder.Entity<Therapist>()
          .HasOne(c => c.Account)
          .WithOne(a => a.Therapist)
          .HasForeignKey<Therapist>(v => v.AccountId)
          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CoWorkingSpace>()
          .HasOne(c => c.Account)
          .WithOne(a => a.CoWorkingSpace)
          .HasForeignKey<CoWorkingSpace>(v => v.AccountId)
          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Credentials>()
         .HasOne(c => c.Therapist)
         .WithMany(a => a.Credentials)
         .HasForeignKey(v => v.TherapistId)
         .OnDelete(DeleteBehavior.Cascade);

        }
        public MMDbContext(DbContextOptions<MMDbContext> options) : base(options)
        {

        }
    }
}

