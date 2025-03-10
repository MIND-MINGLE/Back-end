using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class PatientSurveyConfig : IEntityTypeConfiguration<PatientSurvey>
    {
        public PatientSurveyConfig()
        {
        }
        public void Configure(EntityTypeBuilder<PatientSurvey> builder)
        {
            // Primary key
            builder.HasKey(a => a.PatientSurveyId);

            // One-to-one relationship
            builder.HasOne(a => a.Patient)
                .WithMany(r => r.PatientSurveys)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Patient)
               .WithMany(r => r.PatientSurveys)
               .HasForeignKey(a => a.PatientId)
               .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(a => a.PatientResponses)
               .WithOne(r => r.PatientSurvey)
               .HasForeignKey(a => a.PatientSurveyId)
               .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(a => a.InCategories)
              .WithOne(r => r.PatientSurvey)
              .HasForeignKey(a => a.PatientSurveyId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

