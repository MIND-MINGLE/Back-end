using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class RatingConfig:IEntityTypeConfiguration<Rating>
	{
		public RatingConfig()
		{
		}
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(a => a.RatingId);
            builder.HasOne(r => r.Patient)
            .WithMany(t => t.Ratings)
            .HasForeignKey(s => s.PatientId);
            builder.HasOne(r => r.Appointment)
          .WithOne(t => t.Ratings)
          .HasForeignKey<Rating>(s => s.AppointmentId);
        }
    }
}

