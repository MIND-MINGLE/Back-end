using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request.Account
{
	public class GoogleLoginRequest
	{
		public string Email { get; set; } = string.Empty;
		public bool Email_verified { get; set; } = false;
        public string Family_name { get; set; } = string.Empty;
        public string Given_name { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string Sub { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
    }
}
