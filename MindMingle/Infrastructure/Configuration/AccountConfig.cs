using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class AccountConfig : IEntityTypeConfiguration<Account>
	{
		public AccountConfig()
		{
		}

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Primary key
            builder.HasKey(a => a.AccountId);
            // One-to-one relationship
            builder.HasOne(a => a.Role)
                .WithOne(r => r.Account)
                .HasForeignKey<Account>(a => a.RoleId);
        }
    }
}

