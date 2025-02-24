using System;
namespace Domain.Entity
{
	public class UsersInGroup : Norms
	{
        required public string UsersInGroupId { get; set; }
        required public string ClientId { get; set; }
        required public string ChatGroupId { get; set; }
        //
        public ChatGroup ChatGroup { get; set; } = null!; //Navigation property
        public Account Account { get; set; } = null!;
        public ICollection<ChatMessage> ChatMessages { get; set; } = null!;
    }
}

