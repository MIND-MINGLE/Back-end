using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Therapist : Norms
    {
        [Key]
        required public string TherapistId { get; set; }
        public required string AccountId { get; set; } // Fk
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
        [Required]
        public required DateTime Dob { get; set; }
        [Required]
        public required string Gender { get; set; }
        public required double PricePerHour { get; set; }

        // Link between Account and Therapist
        public Account Account { get; set; } = null!;
        public ICollection<Credentials>? Credentials{get;set;}
        public ICollection<Session>? Sessions { get; set; } // one-many
        public ICollection<Appointment>? Appointment { get; set; } // one-many
    }
}

