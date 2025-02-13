using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Account
{
	public class UserRegisterRequest
	{
		required public string Email { get; set; }
		required public string AccountName { get; set; }
		required public string Password { get; set; }
		required public string ConfirmPassword { get; set; }	
	}
}
