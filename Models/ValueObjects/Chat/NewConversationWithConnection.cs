using OnlineConsulting.Models.Entities;

namespace OnlineConsulting.Models.ValueObjects.Chat
{
    public class NewConversationWithConnection
    {
        public string ConnectionId { get; set; }
        public Conversation Conversation { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
