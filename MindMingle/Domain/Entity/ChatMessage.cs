using System;
namespace Domain.Entity
{
    public class ChatMessage : Norms
    {
        required public string ChatMessageId { get; set; }
        required public string AccountId { get; set; }
        required public string UsersInGroupId { get; set; }
        required public string Content { get; set; }
        required public MessageStatus MessageStatus { get; set; }

        public Account Account { get; set; } = null!;
        public UsersInGroup UsersInGroup { get; set; } = null!;
       
    }
}

