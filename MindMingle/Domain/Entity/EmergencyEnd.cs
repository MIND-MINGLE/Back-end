using System.ComponentModel.DataAnnotations;
using Domain.Entity;

namespace Domain.Entity
{
    public class EmergencyEnd
    {
        [Key]
        public required string  EmergencyEndId { get; set; }

        [Required]
        public required string AppointmentId { get; set; }

        [Required]
        public required string AccountId { get; set; }

        [Required]
        public required string Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public Appointment Appointment { get; set; } = null!;
    }
}