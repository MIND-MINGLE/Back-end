using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
    {
        public AppointmentConfig()
        {
        }
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.AppointmentId);
            builder.HasOne(em => em.EmergencyEnd)
             .WithOne(ap => ap.Appointment)
             .HasForeignKey<EmergencyEnd>(em => em.AppointmentId);
            builder.HasOne(ap => ap.Patient)
            .WithMany(p => p.Appointment)
            .HasForeignKey(ap => ap.PatientId);
            builder.HasOne(ap => ap.CoWorkingSpace)
            .WithMany(p => p.Appointment)
            .HasForeignKey(ap => ap.CoWorkingSpaceId);
            builder.HasOne(ap => ap.Therapist)
            .WithMany(t => t.Appointment)
            .HasForeignKey(ap => ap.TherapistId);
            builder.HasOne(ap => ap.ChatGroup)
            .WithOne(t => t.Appointment)
            .HasForeignKey<Appointment>(ap => ap.GroupChatId);
            builder.HasOne(ap => ap.Ratings)
         .WithOne(t => t.Appointment)
         .HasForeignKey<Rating>(ap => ap.AppointmentId);
        }
    }
}

