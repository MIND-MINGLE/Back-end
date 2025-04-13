using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Domain.Entity;
using Newtonsoft.Json.Linq;

namespace Domain.Entity
{
    public class Appointment:Norms
    {
        [Key]
        public required string AppointmentId { get; set; }

        [Required]
        public required string PatientId { get; set; }

        [Required]
        public required string TherapistId { get; set; }

        public string? CoWorkingSpaceId { get; set; } // Nullable FK

        public required string SessionId { get; set; } 

        public string? EmergencyEndId { get; set; }
        public required string GroupChatId { get; set; }

        [Required]
        public AppointmentType AppointmentType { get; set; } // e.g., "Online", "Offline"

        [Required]
        public Status Status { get; set; } // e.g., "Scheduled", "Completed"

        [Required]
        public double TotalFee { get; set; }

        public double PlatformFee { get; set; } // 20% of TotalFee

        // Navigation properties
        public ChatGroup ChatGroup { get; set; } = null!;
        public Therapist Therapist { get; set; } = null!;
        public Session Session { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public CoWorkingSpace? CoWorkingSpace { get; set; }
        public EmergencyEnd? EmergencyEnd { get; set; }
        public Payment? Payments { get; set; }
        public Rating? Ratings { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AppointmentType
    {
        [EnumMember(Value = "Offline")]
        OFFLINE,
        [EnumMember(Value = "Online")]
        ONLINE,
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "Pending")]
        PENDING,
        [EnumMember(Value = "Approved")]
        APPROVED,
        [EnumMember(Value = "Ended")]
        ENDED,
        [EnumMember(Value = "Declined")]
        DECLINED,
        [EnumMember(Value = "Canceled")]
        CANCELED,
        [EnumMember(Value = "Overdue")]
        OVERDUE,

    }
}