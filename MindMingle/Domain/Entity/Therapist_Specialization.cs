using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Therapist_Specialization:Norms	{
        [Key]
        public required string Therapist_SpecializationId { get; set; }
        [Required]
        public required string TherapistId { get; set; }
        [Required]
        public required string SpecializationId { get; set; }

        public Therapist Therapist { get; set; } = null!;
        public Specialization Specialization { get; set; } = null!;
    }
}

