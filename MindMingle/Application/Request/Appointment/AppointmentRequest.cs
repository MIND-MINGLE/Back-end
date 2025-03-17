using System;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Domain.Entity;

namespace Application.Request.Appointment
{
	public class AppointmentRequest
	{
        [Required]
        public required string PatientId { get; set; }

        [Required]
        public required string TherapistId { get; set; }

        public string? CoWorkingSpaceId { get; set; } // Nullable FK

        public required string SessionId { get; set; }

        public string EmergencyEndId { get; set; }

        public AppointmentType AppointmentType { get; set; } // e.g., "Online", "Offline"


        // public Status Status { get; set; } // e.g., "Scheduled", "Completed" // Add this in mapper

        public double TotalFee { get; set; }

        public double PlatformFee { get; set; } // 20% of TotalFee
    }
}