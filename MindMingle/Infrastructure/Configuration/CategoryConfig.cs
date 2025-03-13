using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public CategoryConfig()
        {
        }
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);
            builder.HasMany(p => p.Questions)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);
        }
    }
}

