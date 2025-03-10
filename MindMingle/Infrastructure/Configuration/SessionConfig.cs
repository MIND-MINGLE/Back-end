using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class SessionConfig : IEntityTypeConfiguration<Session>
    {
        public SessionConfig()
        {
        }
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(a => a.SessionId);
            builder.HasMany(r => r.Appointments)
             .WithOne(ap => ap.Session)
             .HasForeignKey(ap=>ap.SessionId);
            builder.HasOne(r => r.Therapist)
            .WithMany(t => t.Sessions)
            .HasForeignKey(s => s.TherapistId);
        }
    }
}

