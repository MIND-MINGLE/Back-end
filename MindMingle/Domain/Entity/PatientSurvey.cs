using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class PatientSurvey
    {
        [Key]
        public required string PatientSurveyId { get; set; }

        public required string PatientId { get; set; }

        public DateTime CreatedAt { get; set; }

        public required string Summary { get; set; }

        // Navigation properties
        public Patient Patient { get; set; } = null!;
        public ICollection<PatientResponse> PatientResponses { get; set; } = null!; // One-to-many
      
    }
}