using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class SubscriptionConfig: IEntityTypeConfiguration<Subscription>
    {
		public SubscriptionConfig()
		{
		}
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(a => a.SubscriptionId);
            builder.HasMany(r => r.PurchasedPackages)
             .WithOne(ap => ap.Subscription)
             .HasForeignKey(ap => ap.SubscriptionId);
        }
    }
}

