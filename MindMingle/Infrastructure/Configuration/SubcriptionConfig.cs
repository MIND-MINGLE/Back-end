using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class SubcriptionConfig: IEntityTypeConfiguration<Subcription>
    {
		public SubcriptionConfig()
		{
		}
        public void Configure(EntityTypeBuilder<Subcription> builder)
        {
            builder.HasKey(a => a.SubcriptionId);
            builder.HasMany(r => r.PurchasedPackages)
             .WithOne(ap => ap.Subcription)
             .HasForeignKey(ap => ap.SubscriptionId);
        }
    }
}

