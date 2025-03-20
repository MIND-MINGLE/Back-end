using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class PurchasedPackageConfig: IEntityTypeConfiguration<PurchasedPackage>
    {
		public PurchasedPackageConfig()
		{
		}
        public void Configure(EntityTypeBuilder<PurchasedPackage> builder)
        {
            builder.HasKey(a => a.PurchasedPackageId);
            builder.HasOne(r => r.Patient)
             .WithMany(ap => ap.PurchasedPackages)
             .HasForeignKey(ap => ap.PatientId);
            builder.HasOne(r => r.Subscription)
            .WithMany(t => t.PurchasedPackages)
            .HasForeignKey(s => s.SubscriptionId);
        }
    }
}

