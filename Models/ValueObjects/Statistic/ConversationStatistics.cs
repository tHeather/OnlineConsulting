using System.Collections.Generic;

namespace OnlineConsulting.Models.ValueObjects.Statistic
{
    public class ConversationStatistics
    {
        public IEnumerable<DailyConversationsNumber> AllConversations { get; set; }
        public IEnumerable<DailyConversationsNumber> ServedConversations { get; set; }
        public IEnumerable<DailyConversationsNumber> NotServedConversations { get; set; }
    }
}
