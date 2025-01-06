using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class PatientConfig : IEntityTypeConfiguration<Patient>
	{
		public PatientConfig()
		{
		}

        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(c => c.PatientId);
            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.Property(c => c.Gender).IsRequired();
            builder.Property(c => c.Dob).IsRequired();
            builder.Property(c => c.PhoneNumber).IsRequired();
            builder.Property(c => c.IsDisabled).IsRequired();
            builder.Property(c => c.UpdatedAt).IsRequired();
            builder.HasOne(p => p.Account)
                .WithOne(a => a.Patient)
                .HasForeignKey<Patient>(a => a.AccountId);
        }
    }
}

