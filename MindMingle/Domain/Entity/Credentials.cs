using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
	public class Credentials : Norms
	{
        [Key]
        required public string CredentialsId { get; set; }
        required public string ImageURL {set;get;}
        required public string TherapistId { get; set; }

        public Therapist Therapist { get; set; } = null!;
    }
}

