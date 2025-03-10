using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class InCategory
    {
        [Key]
        public required string InCategoryId { get; set; }

        public required string PatientSurveyId { get; set; }

        public required string CategoryId { get; set; }

        // Navigation properties
        public PatientSurvey PatientSurvey { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}