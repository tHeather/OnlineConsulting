using System;
using System.Collections.Generic;

namespace OnlineConsulting.Models.ValueObjects.Statistic
{
    public class ConversationStatistics
    {
        public IEnumerable<DailyConversationsNumber> AllConversations { get; set; }
        public IEnumerable<DailyConversationsNumber> ServedConversations { get; set; }
        public IEnumerable<DailyConversationsNumber> NotServedConversations { get; set; }
        public TimeSpan AverageTimeConsultantJoining { get; set; }
        public TimeSpan AverageConversationDuration { get; set; }
        public int InProgressConversationsNumber { get; set; }
        public int NewConversationsNumber { get; set; }
    }
}
