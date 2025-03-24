using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class SpecializationConfig : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.HasKey(c => c.SpecializationId);
            builder.HasMany(t => t.Therapist_Specializations)
             .WithOne(t => t.Specialization)
             .HasForeignKey(t => t.TherapistId);
        }

    }
}

