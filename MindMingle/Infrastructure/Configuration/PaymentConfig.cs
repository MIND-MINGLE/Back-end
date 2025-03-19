using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class PaymentConfig: IEntityTypeConfiguration<Payment>
    {
		public PaymentConfig()
		{
		}
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(a => a.PaymentId);
            builder.HasOne(r => r.Patient)
            .WithMany(t => t.Payments)
            .HasForeignKey(s => s.PatientId);
            builder.HasOne(r => r.Appointment)
          .WithOne(t => t.Payments)
          .HasForeignKey<Payment>(s => s.AppointmentId);
        }
    }
}

