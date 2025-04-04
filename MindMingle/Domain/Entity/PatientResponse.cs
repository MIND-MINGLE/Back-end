using System.ComponentModel.DataAnnotations;


namespace Domain.Entity
{
    public class PatientResponse
    {
        [Key]
        public required string PatientResponseId { get; set; }
        public string? PatientSurveyId { get; set; }
        public required string QuestionId { get; set; }
        public required string AnswerId { get; set; }
        public string? CustomAnswer { get; set; } // Nullable field
        public int Score { get; set; }
        // Navigation properties
        public PatientSurvey? PatientSurvey { get; set; }
        public Question Question { get; set; } = null!;
        public Answer Answer { get; set; } = null!;
    }
}