using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entity;

namespace Application.Request.Session
{
	public class CreateSessionRequest
	{

        [Required]
        public required string TherapistId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }


        public required DaysOfWeek DayOfWeek { get; set; } // e.g., "Monday"
    }
   
}

