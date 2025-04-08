using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Rating:Norms
	{
        [Key]
        public required string RatingId { get; set; }
        [Required]
        required public string TherapistId { get; set; }
        [Required]
        public required string PatientId { get; set; }
        [Required]
        public required string AppointmentId { get; set; }
        [Required]
        public required string Comment { get; set; }
        [Required]
        public required double Score { get; set; }

        public required Patient Patient { get; set; }
        public required Appointment Appointment { get; set; }
        public required Therapist Therapist { get; set; }
    }
}

