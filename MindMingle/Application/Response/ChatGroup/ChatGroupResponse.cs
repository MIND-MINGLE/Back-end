using System;
namespace Application.Response.ChatGroup
{
	public class ChatGroupResponse
	{
        required public string Id { get; set; }
        required public string AdminId { get; set; }
        required public string AdminName { get; set; }
    }
}

