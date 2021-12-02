using OnlineConsulting.Enums;
using System;

namespace OnlineConsulting.Models.ValueObjects.Chat
{
    public class ConversationFilters
    {
        public ConversationStatus?  Status{ get; set; }
        public DateTime? EndDateUtc { get; set; }
        public DateTime? StartDateUtc { get; set; }
        public string ConsultantEmail { get; set; }
        public string Host { get; set; }
    }
}
