using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class QuestionConfig : IEntityTypeConfiguration<Question>
    {
		public QuestionConfig()
		{
		}
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(c => c.QuestionId);
        
            builder.HasMany(p => p.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);
            builder.HasMany(p => p.PatientResponses)
               .WithOne(a => a.Question)
               .HasForeignKey(a => a.QuestionId);
        }
    }
}

