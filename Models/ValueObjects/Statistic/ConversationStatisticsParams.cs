using System;

namespace OnlineConsulting.Models.ValueObjects.Statistic
{
    public class ConversationStatisticsParams
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Domain { get; set; }
    }
}
