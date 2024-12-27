using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Role
	{
        [Key]
        required public string RoleId { get; set; }
        required public string RoleName { get; set; }

        public Account Account { get; set; } = null!;
	}
}

