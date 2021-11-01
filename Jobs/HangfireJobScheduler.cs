using Hangfire;
using System;
using System.Threading;

namespace OnlineConsulting.Jobs
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecuringJobs()
        {
            RecurringJob.RemoveIfExists(nameof(CloseUnusedConversationsJob));
            RecurringJob.AddOrUpdate<CloseUnusedConversationsJob>(nameof(CloseUnusedConversationsJob),
                         job => job.Run(CancellationToken.None), "*/15 * * * *", TimeZoneInfo.Utc);
        }
    }
}
