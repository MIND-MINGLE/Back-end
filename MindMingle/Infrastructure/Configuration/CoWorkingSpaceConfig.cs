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
            // One-to-one relationship
            builder.HasOne(a => a.Account)
                .WithOne(r => r.CoWorkingSpace)
                .HasForeignKey<CoWorkingSpace>(a => a.AccountId);
        }
    }
}

