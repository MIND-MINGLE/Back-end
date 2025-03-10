using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class AnswerConfig : IEntityTypeConfiguration<Answer>
    {
        public AnswerConfig()
        {
        }
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(c => c.AnswerId);

            builder.HasOne(p => p.Question)
                .WithMany(a => a.Answers)
                .HasForeignKey(a => a.QuestionId);
            builder.HasMany(p => p.PatientResponses)
               .WithOne(a => a.Answer)
               .HasForeignKey(a => a.AnswerId);
        }
    }
}

