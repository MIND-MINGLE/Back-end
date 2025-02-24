using System;
namespace Application.Response.UsersInGroup
{
	public class GetAllUserInGroupResponse
	{
        required public string ChatGroupId { get; set; }
        required public string ClientId { get; set; }
        required public string AccountName { get; set; }
    }
}

