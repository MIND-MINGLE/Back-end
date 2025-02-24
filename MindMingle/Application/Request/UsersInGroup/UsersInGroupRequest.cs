using System;
namespace Application.Request.UsersInGroup
{
	public class UsersInGroupRequest
	{
		public required string GroupId { get; set; }
		public required string ClientId { get; set; }
	}
}

