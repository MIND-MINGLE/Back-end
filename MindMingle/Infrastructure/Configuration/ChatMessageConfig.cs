using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class ChatMessageConfig : IEntityTypeConfiguration<ChatMessage>
	{
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            // Primary key
            builder.HasKey(a => a.ChatMessageId);
            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();
            builder.Property(a => a.IsDisabled);

            // One-to-Many relationship
            builder.HasOne(a => a.Account)
                .WithMany(r => r.ChatMessages)
                .HasForeignKey(a => a.ClientId);
        }
    }
}

