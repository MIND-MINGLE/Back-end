using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Specialization:Norms
	{
        [Key]
        public required string SpecializationId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }

        public ICollection<Therapist_Specialization>? Therapist_Specializations { get; set; }
    }
}

