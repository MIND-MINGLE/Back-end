using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Request.Therapist
{
	public class AddNewTherapistRequest
	{
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
        [Required]
        public required double PricePerHour { get; set; }
    }
}

