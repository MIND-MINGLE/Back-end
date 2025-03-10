using System.ComponentModel.DataAnnotations;


namespace Domain.Entity
{
    public class PatientResponse
    {
        [Key]
        public required string PatientResponseId { get; set; }
        public required string PatientSurveyId { get; set; }
        public required string QuestionId { get; set; }
        public required string AnswerId { get; set; }
        public string? CustomerAnswer { get; set; } // Nullable field
        public int Score { get; set; }
        // Navigation properties
        public PatientSurvey PatientSurvey { get; set; } = null!;
        public Question Question { get; set; } = null!;
        public Answer Answer { get; set; } = null!;
    }
}