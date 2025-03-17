using System;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Application.Response
{
	public class ResponseSession
	{
        [Key]
        public required string SessionId { get; set; }

        [Required]
        public required string TherapistId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public required DaysOfWeek DayOfWeek { get; set; } // e.g., "Monday"

        public bool IsActive { get; set; }
    }
}

