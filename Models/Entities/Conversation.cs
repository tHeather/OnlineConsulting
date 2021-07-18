using OnlineConsulting.Enums;
using System;
using System.Collections.Generic;


namespace OnlineConsulting.Models.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid LastMessageId { get; set; }
        public string ConsultantId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ConversationStatus Status { get; set; }

        public ChatMessage LastMessage { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }
        public User Consultant { get; set; }
    }
}
