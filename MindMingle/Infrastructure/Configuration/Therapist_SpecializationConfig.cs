using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class Therapist_SpecializationConfig: IEntityTypeConfiguration<Therapist_Specialization>
    {
        public Therapist_SpecializationConfig()
        {

        }
        public void Configure(EntityTypeBuilder<Therapist_Specialization> builder)
        {
            builder.HasKey(c => c.SpecializationId);
            builder.HasOne(r => r.Therapist)
          .WithMany(ap => ap.Therapist_Specializations)
          .HasForeignKey(ap => ap.TherapistId);
            builder.HasOne(r => r.Specialization)
          .WithMany(ap => ap.Therapist_Specializations)
          .HasForeignKey(ap => ap.SpecializationId);
        }

    }
}

