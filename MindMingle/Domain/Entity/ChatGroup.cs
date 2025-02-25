using System;
namespace Domain.Entity
{
	public class ChatGroup : Norms
	{
        required public string Id { get; set; }
        required public string AdminId { get; set; }
        
        public Account Account { get; set; } = null!;
        public ICollection<UsersInGroup>? UsersInGroups { get; set; } // As Client
       
    }

}

