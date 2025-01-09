using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Role
	{
        [Key]
        required public string RoleId { get; set; }
        required public string RoleName { get; set; }

        public ICollection<Account> Account { get; set; } = null!;
	}
}

