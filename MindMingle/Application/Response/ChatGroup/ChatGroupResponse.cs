using System;
namespace Application.Response.ChatGroup
{
	public class ChatGroupResponse
	{
        required public string ChatGroupId { get; set; }
        required public string AdminId { get; set; }
        required public string AdminName { get; set; }
        required public string UserInGroupId { get; set; }
        public bool IsDisabled { get; set; }

    }
}

