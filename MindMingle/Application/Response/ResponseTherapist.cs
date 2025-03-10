using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Response
{
	public class ResponseTherapist
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
        public required string Dob { get; set; }
        [Required]
        public required string Gender { get; set; }
    }
}

