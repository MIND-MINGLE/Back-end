using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entity
{
	public class Account : Norms
	{
        [Key]
        required public string AccountId { get; set; } 
        required public string AccountName { get; set; }
		required public string Password { get; set; }
		required public string RoleId { get; set; }
		required public string Email { get; set; }
		public string? Avatar { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.Now;
        public bool? IsEmailVerified { get; set; } = false;

		public Role Role { get; set; } = null!;
		public Patient? Patient { get; set; }
        public Therapist? Therapist { get;  set; }
        public CoWorkingSpace? CoWorkingSpace { get; set; }

        public ICollection<ChatMessage>? ChatMessages { get; set; }
        
        public ICollection<UsersInGroup>?  UsersInGroups{ get; set; } // As Client
     
        public ICollection<ChatGroup>? ChatGroups { get; set; } // As Admin

    }
}