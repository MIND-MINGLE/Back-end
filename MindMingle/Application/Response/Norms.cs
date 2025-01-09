using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Response
{
	public class Norms
	{
        public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
		public bool IsDisabled { get; set; } 
    }
}

