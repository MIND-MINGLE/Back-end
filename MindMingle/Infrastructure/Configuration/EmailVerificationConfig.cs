using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class EmailVerificationConfig : IEntityTypeConfiguration<EmailVerification>
    {
        public EmailVerificationConfig()
        {
        }

        public void Configure(EntityTypeBuilder<EmailVerification> builder)
        {
            builder.HasKey(c => c.VerificationId);
            // One - Many
            builder.HasOne(t => t.Account)
                .WithOne(c => c.EmailVerification)
                .HasForeignKey<EmailVerification>(c => c.AccountId);
        }
    }
}

