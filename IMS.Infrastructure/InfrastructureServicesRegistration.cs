using IMS.Application.Interfaces.Infrastructure;
using IMS.Infrastructure.Services.Cache;
using IMS.Infrastructure.Services.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        var redisConfig = new StackExchange.Redis.ConfigurationOptions
        {
            EndPoints = { configuration["Redis:Endpoint"]! },
            User = configuration["Redis:User"]!,
            Password = configuration["Redis:Password"]!,
            AbortOnConnectFail = false
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
            var hostName = configuration["RabbitMQ:Host"]!;
            return RabbitMqEventBus.CreateAsync(hostName).GetAwaiter().GetResult();
        });

        return services;
    }
}
