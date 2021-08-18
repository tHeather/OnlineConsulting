using OnlineConsulting.Models.Entities;

namespace OnlineConsulting.Models.ValueObjects.Chat
{
    public class CreateMessage
    {
        public string Content { get; set; }
        public Conversation Conversation { get; set; }
        public bool IsFromClient { get; set; }
    }
}
