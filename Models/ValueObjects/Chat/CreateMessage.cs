using System;

namespace OnlineConsulting.Models.ValueObjects.Chat
{
    public class CreateMessage
    {
        public string Content { get; set; }
        public Guid ConversationId { get; set; }
        public bool IsFromClient { get; set; }
    }
}
