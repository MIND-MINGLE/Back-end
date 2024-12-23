using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Norms
	{
        [Key]
        required public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public bool IsDisabled { get; set; } 
    }
}

