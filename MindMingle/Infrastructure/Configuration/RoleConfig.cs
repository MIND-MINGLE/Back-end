using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
	public class RoleConfig : IEntityTypeConfiguration<Role>
	{
		public RoleConfig()
		{
		}

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(a => a.RoleId);
            builder.Property(a => a.RoleName).IsRequired();
            //
            //builder.HasOne(r => r.Account)
            //  .WithOne(r => r.Role);
        }
    }
}

