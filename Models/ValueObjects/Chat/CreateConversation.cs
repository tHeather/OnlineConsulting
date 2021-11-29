using System;

namespace OnlineConsulting.Models.ValueObjects.Chat
{
    public class CreateConversation
    {
        public string Host { get; set; }
        public string Path { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}
