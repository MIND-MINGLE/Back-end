using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class MMDbContext : DbContext
	{
		public MMDbContext()
		{
		}
		public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<CoWorkingSpace> CoWorkingSpaces { get; set; }
    }
}

