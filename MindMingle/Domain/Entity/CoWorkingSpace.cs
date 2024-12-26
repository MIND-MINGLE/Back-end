using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class CoWorkingSpace:Norms
	{
        public required string AccountId { get; set; } // FK
        [Required]
        public required string AgentName { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
        [Required]
        public required string Address { get; set; }

        // Link between Account and RentalService
        public Account Account { get; set; } = null!;
    }
}

