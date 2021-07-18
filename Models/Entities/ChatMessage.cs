using System;

namespace OnlineConsulting.Models.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string ConsultantId { get; set; }
        public string Origin { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

        public User Consultant { get; set; }
    }
}
