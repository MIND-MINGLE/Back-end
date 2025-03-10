using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Answer
    {
        [Key]
        public required string AnswerId { get; set; }
        public required string QuestionId { get; set; }
        public required string AnswerContent { get; set; }

        public int Score { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Question Question { get; set; } = null!;
        public ICollection<PatientResponse>? PatientResponses { get; set; } // One-to-many
    }
}