using IMS.Application.Interfaces.Infrastructure;
using IMS.Infrastructure.Services.Cache;
using IMS.Infrastructure.Services.MessageBroker;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
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

        services.AddSingleton<IEventBus>(sp =>
        {
            return RabbitMqEventBus.CreateAsync().GetAwaiter().GetResult();
        });

        return services;
    }
}
