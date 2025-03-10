using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class PatientResponseConfig : IEntityTypeConfiguration<PatientResponse>
    {
        public PatientResponseConfig()
        {
        }
        public void Configure(EntityTypeBuilder<PatientResponse> builder)
        {
            builder.HasKey(c => c.PatientResponseId);

            builder.HasOne(p => p.PatientSurvey)
                .WithMany(a => a.PatientResponses)
                .HasForeignKey(a => a.PatientSurveyId);
        }
    }
}

