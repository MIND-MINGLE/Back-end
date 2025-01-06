using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class CoWorkingSpaceConfig : IEntityTypeConfiguration<CoWorkingSpace>
	{
		public CoWorkingSpaceConfig()
		{
		}

        public void Configure(EntityTypeBuilder<CoWorkingSpace> builder)
        {
            // Primary key
            builder.HasKey(a => a.CoWorkingSpaceId);
            builder.Property(c => c.AgentName).IsRequired();
            builder.Property(c => c.PhoneNumber).IsRequired();
            builder.Property(c => c.IsDisabled);
            // One-to-one relationship
            builder.HasOne(a => a.Account)
                .WithOne(r => r.CoWorkingSpace)
                .HasForeignKey<CoWorkingSpace>(a => a.AccountId);
        }
    }
}

