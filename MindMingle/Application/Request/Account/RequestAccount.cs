using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Request.Account
{
	public class RequestAccount
	{
         public string AccountId { get; set; }
         public string AccountName { get; set; }
         public string Password { get; set; }
         public string RoleId { get; set; }
         public string Email { get; set; }
    }
}

