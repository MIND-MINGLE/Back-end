using System;
using Domain.Entity;

namespace Application.Response
{
	public class ResponseAccount : Norms
	{
        public string AccountId { get; set; }
        public string AccountName { get; set; }
         public string Password { get; set; }
         public string RoleId { get; set; }
         public string Email { get; set; }
        public string? Avatar { get; set; }
        public DateTime LastLogin { get; set; }
    }
}

