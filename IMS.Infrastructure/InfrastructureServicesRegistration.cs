using IMS.Application.Interfaces.Infrastructure;
using IMS.Infrastructure.ExternalServices;
using IMS.Infrastructure.Services.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace IMS.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpClient<INotificationService, HttpBinNotificationService>(options =>
        {
            options.BaseAddress = new Uri("https://httpbin.org/");
        })
        .AddStandardResilienceHandler(options =>
        {
            options.Retry = new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromSeconds(1)
            };

            options.CircuitBreaker = new HttpCircuitBreakerStrategyOptions
            {
                BreakDuration = TimeSpan.FromSeconds(30),
                FailureRatio = 0.5,
                MinimumThroughput = 10,
                SamplingDuration = TimeSpan.FromSeconds(60)
            };

            options.AttemptTimeout = new HttpTimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            options.TotalRequestTimeout = new HttpTimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(12)
            };
        });

        services.AddMemoryCache();
        var redisConfig = new StackExchange.Redis.ConfigurationOptions
        {
            EndPoints = { "redis-10964.c56.east-us.azure.cloud.redislabs.com:10964" },
            User = "default",
            Password = "LS2oGZjS7RAopazHAhtPU3FMB3yfIPKg"
        };

        services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfig));

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = redisConfig;
        });
        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}
