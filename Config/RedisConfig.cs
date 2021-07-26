using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineConsulting.Constants;
using StackExchange.Redis;

namespace OnlineConsulting.Config
{
    public static class RedisConfig
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration[Parameters.REDIS_CONNECTION_STRING] ?? "localhost"));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration[Parameters.REDIS_CONNECTION_STRING];
                options.InstanceName = "AspNetRateLimit";
            });

            return services;
        }
    }
}
