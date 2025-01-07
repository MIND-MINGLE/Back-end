using System;
namespace Domain.Entity
{
    public class ChatMessage : Norms
    {
        required public string ChatMessageId { get; set; }
        required public string ClientId { get; set; }
        required public string ChatGroupId { get; set; }
        required public string Content { get; set; }
        required public MessageStatus MessageStatus { get; set; }

        public Account Account { get; set; } = null!;
        public ChatGroup ChatGroup { get; set; } = null!;
      
    }
}

