using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Account
{
	public class UserRegisterRequest
	{
		public string AccountName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }	
		public string RoleId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
	}
}
