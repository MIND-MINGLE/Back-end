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
            builder.Property(a=>a.AccountName).IsRequired();
            builder.Property(a => a.Password).IsRequired();
            builder.Property(a => a.Avatar);
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();
            builder.Property(a => a.IsDisabled);
            builder.Property(a => a.LastLogin);
      
            // One-to-one relationship
            builder.HasOne(a => a.Role)
                .WithMany(r => r.Account)
                .HasForeignKey(a => a.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

