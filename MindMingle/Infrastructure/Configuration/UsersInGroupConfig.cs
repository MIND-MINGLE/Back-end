using System;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configuration
{
	public class UsersInGroupConfig: IEntityTypeConfiguration<UsersInGroup>
    {
		public UsersInGroupConfig()
		{
		}
        public void Configure(EntityTypeBuilder<UsersInGroup> builder)
        {
            // Primary key
            builder.HasKey(a => a.UsersInGroupId);
            builder.Property(a => a.ChatGroupId).IsRequired();
            builder.Property(a => a.ClientId).IsRequired();
            builder.Property(a => a.IsDisabled);

            // One-to-M relationship
            builder.HasOne(a => a.Account)
              .WithMany(r => r.UsersInGroups)
              .HasForeignKey(a => a.ClientId)
              .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.ChatGroup)
              .WithMany(r => r.UsersInGroups)
              .HasForeignKey(a => a.ChatGroupId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

