using System;
namespace Application.Response
{
	public class ResponseAccount : Norms
	{
        required public string AccountName { get; set; }
        required public string Password { get; set; }
        required public int RoleId { get; set; }
        required public string Email { get; set; }
        public string? Avatar { get; set; }
        public DateTime LastLogin { get; set; }
    }
}

