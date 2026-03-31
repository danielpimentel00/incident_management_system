using IMS.GrpcService.Protos;
using IMS.Jobs.ExternalServices;
using IMS.Jobs.Interfaces;
using IMS.Jobs.Services;
using Microsoft.Extensions.Http.Resilience;
using Polly;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<IncidentEscalationService>();
builder.Services.AddHostedService<ApiHealthCheckService>();
builder.Services.AddHostedService<IncidentEscalationConsumer>();

builder.Services.AddGrpcClient<IncidentService.IncidentServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7286");
});

builder.Services.AddHttpClient("ApiHealthCheck", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHttpClient<INotificationService, HttpBinNotificationService>(options =>
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

var host = builder.Build();
host.Run();
