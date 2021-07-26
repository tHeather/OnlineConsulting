using OnlineConsulting.Models.Entities;

namespace OnlineConsulting.Models.ValueObjects.Chat
{
    public class CreateMessage
    {
        public string Origin { get; set; }
        public string Content { get; set; }
        public Conversation Conversation { get; set; }
    }
}
