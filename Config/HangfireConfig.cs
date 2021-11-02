using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineConsulting.Jobs;
using System;

namespace StudyOnlineServer.Config
{
    public static class HangfireConfig
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));
            services.AddHangfireServer(opt =>
            {
                opt.HeartbeatInterval = new TimeSpan(0, 1, 0);
                opt.ServerCheckInterval = new TimeSpan(0, 1, 0);
                opt.SchedulePollingInterval = new TimeSpan(0, 1, 0);
                opt.WorkerCount = 5;
            });
            services.AddScoped<IJob, CloseUnusedConversationsJob>();

            return services;
        }
    }
}
