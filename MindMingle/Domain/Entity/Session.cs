using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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


        public required DaysOfWeek DayOfWeek { get; set; } // e.g., "Monday"

        public bool IsActive { get; set; }

        // Navigation property
        public ICollection<Appointment>? Appointments { get; set; } // One-to-many
        public Therapist Therapist { get; set; } = null!;
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DaysOfWeek
    {
        [EnumMember(Value = "Monday")]
        MONDAY,
        [EnumMember(Value = "Tuesday")]
        TUESDAY,
        [EnumMember(Value = "Wednesday")]
        WEDNESDAY,
        [EnumMember(Value = "Thursday")]
        THURSDAY,
        [EnumMember(Value = "Friday")]
        FRIDAY,
        [EnumMember(Value = "Saturday")]
        SATURDAY,
        [EnumMember(Value = "Sunday")]
        SUNDAY,
    }
}