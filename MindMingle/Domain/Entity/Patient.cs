using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Patient : Norms
    {
        public required string AccountId { get; set; } // FK
        [Required]
        public required string PatientName { get; set; }

        [Required]
        public required DateOnly Dob { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        // Link between Account and Patient
        public Account Account { get; set; } = null!;
       
    }
}

