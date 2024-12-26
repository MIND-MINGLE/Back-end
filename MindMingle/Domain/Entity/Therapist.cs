using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Therapist:Norms
	{
        public required string AccountId { get; set; } // Fk
        [Required]
        public required string TherapistName { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
        [Required]
        public required DateOnly Dob { get; set; }
        [Required]
        public required string Gender { get; set; }

        // Link between Account and Therapist
        public Account Account { get; set; } = null!;
    }
}

