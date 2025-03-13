using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace Domain.Entity
{
    public class Question
    {
        [Key]
        public required string QuestionId { get; set; }

        [Required]
        public required string QuestionContent { get; set; }

        [Required]
        public required string Category { get; set; } // Enum field (e.g., "PHQ-9", "GAD-7")

        [Required]
        public required string QuestionType { get; set; } // e.g., "Likert", "YesNo"

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<PatientResponse>? PatientResponses { get; set; } // One-to-many
        public ICollection<Answer>? Answers { get; set; } // One-to-many
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuestionType
    {
        [EnumMember(Value = "PHQ-9")]
        PHQ9,
        [EnumMember(Value = "GAD-7")]
        GAD7,
        [EnumMember(Value = "PC-PTSD-5")]
        PCPTSD5,
    }
}