using System;

namespace OnlineConsulting.Models.ValueObjects.Chat
{
    public class ConsultantChatConnection
    {
        public Guid ConversationId { get; set; }
        public byte[] RowVersion { get; set; }
        public string RedirectAction { get; set; }
    }
}
