using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

namespace Infrastructure.Configuration
{
	public class TherapistConfig : IEntityTypeConfiguration<Therapist>
    {
		public TherapistConfig()
		{
         
        }

        public void Configure(EntityTypeBuilder<Therapist> builder)
        {
            builder.HasKey(c => c.TherapistId);
            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.Property(c => c.Gender).IsRequired();
            builder.Property(c => c.Dob).IsRequired();
            builder.Property(c => c.PhoneNumber).IsRequired();
            builder.Property(c => c.IsDisabled).IsRequired();
            builder.Property(c => c.UpdatedAt).IsRequired();

            builder.HasOne(p => p.Account)
                .WithOne(a => a.Therapist)
                .HasForeignKey<Therapist>(a => a.AccountId);

            builder.HasMany(t => t.Credentials)
                .WithOne(t => t.Therapist)
                .HasForeignKey(t => t.CredentialsId);
        }
    }
}

