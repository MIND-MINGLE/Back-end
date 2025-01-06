using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class CredentialsConfig : IEntityTypeConfiguration<Credentials>
	{
		public CredentialsConfig()
		{

		}

        public void Configure(EntityTypeBuilder<Credentials> builder)
        {
            builder.HasKey(c => c.CredentialsId);
            builder.Property(c => c.ImageURL).IsRequired();
            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.UpdatedAt).IsRequired();
            // One - Many
            builder.HasOne(t => t.Therapist)
                .WithMany(c => c.Credentials)
                .HasForeignKey(c => c.TherapistId);
        }
    }
}

