using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class InCategoryConfig : IEntityTypeConfiguration<InCategory>
    {
        public InCategoryConfig()
        {
        }
        public void Configure(EntityTypeBuilder<InCategory> builder)
        {
            builder.HasKey(c => c.InCategoryId);

            builder.HasOne(cg => cg.PatientSurvey)
                .WithMany(ps => ps.InCategories)
                .HasForeignKey(cg => cg.PatientSurveyId);
            builder.HasOne(cg => cg.Category)
               .WithMany(ps => ps.InCategories)
               .HasForeignKey(cg => cg.CategoryId);



        }
    }
}
