using System;
namespace Application.Request.UsersInGroup
{
	public class UsersInGroupRequest
	{
		public required string ClientId { get; set; }
        public required string ChatGroupId { get; set; }
    }
}

