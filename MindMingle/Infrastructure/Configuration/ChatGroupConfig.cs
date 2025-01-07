using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configuration
{
	public class ChatGroupConfig: IEntityTypeConfiguration<ChatGroup>
    {
		public ChatGroupConfig()
		{
		}
        public void Configure(EntityTypeBuilder<ChatGroup> builder)
        {
            // Primary key
            builder.HasKey(a => a.ChatGroupId);
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();

            // One-to-M relationship
            builder.HasOne(a => a.Account)
                .WithMany(r => r.ChatGroups)
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

