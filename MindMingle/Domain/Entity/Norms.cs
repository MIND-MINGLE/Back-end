using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Norms
	{
        public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; }
		public bool IsDisabled { get; set; } 
    }
}

