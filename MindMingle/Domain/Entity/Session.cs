using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;
namespace Domain.Entity
{
    public class Session:Norms
    {
        [Key]
        public required string SessionId { get; set; }

        [Required]
        public required string TherapistId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }


        public required string DayOfWeek { get; set; } // e.g., "Monday"

        public bool IsActive { get; set; }

        // Navigation property
        public ICollection<Appointment>? Appointments { get; set; } // One-to-many
        public Therapist Therapist { get; set; } = null!;
    }
}