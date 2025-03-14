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
        public required QType QuestionType { get; set; }

        [Required]
        public required string CategoryId { get; set; } // Enum field (e.g., "PHQ-9", "GAD-7")


        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Category Category { get; set; } = null!;
        public ICollection<PatientResponse>? PatientResponses { get; set; } // One-to-many
        public ICollection<Answer>? Answers { get; set; } // One-to-many
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QType
    {
        [EnumMember(Value = "Single")]
        SINGLE,
        [EnumMember(Value = "Multiple")]
        MULTIPLE,
        [EnumMember(Value = "Rating")]
        RATING,
    }


}