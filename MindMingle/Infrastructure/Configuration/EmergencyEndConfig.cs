using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class EmergencyEndConfig : IEntityTypeConfiguration<EmergencyEnd>
    {
        public EmergencyEndConfig()
        {
        }

        public void Configure(EntityTypeBuilder<EmergencyEnd> builder)
        {
            // Primary key
            builder.HasKey(a => a.EmergencyEndId);
          

            // One-to-one relationship
            builder.HasOne(a => a.Appointment)
                .WithOne(r => r.EmergencyEnd)
                .HasForeignKey<EmergencyEnd>(a => a.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
