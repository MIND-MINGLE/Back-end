using System;
namespace Application.Response.ChatGroup
{
	public class ChatGroupResponse
	{
        required public string ChatGroudId { get; set; }
        required public string AdminId { get; set; }
        required public string AdminName { get; set; }
        required public string UserInGroupId { get; set; }
    }
}

