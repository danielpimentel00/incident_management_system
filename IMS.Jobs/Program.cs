using IMS.GrpcService.Protos;
using IMS.Jobs.ExternalServices;
using IMS.Jobs.Interfaces;
using IMS.Jobs.Services;
using Microsoft.Extensions.Http.Resilience;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Serilog;
using Serilog.Enrichers.Span;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(configuration => configuration
    .Enrich.FromLogContext()
    .Enrich.WithSpan()
    .Enrich.WithProperty("Application", "IMS.Jobs")
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"]!)
);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("IMS.Jobs"));
        tracing.AddHttpClientInstrumentation();
        tracing.AddGrpcClientInstrumentation();
        tracing.AddConsoleExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("IMS.Jobs"));
        metrics.AddHttpClientInstrumentation();
        metrics.AddRuntimeInstrumentation();
        metrics.AddPrometheusHttpListener(options => options.UriPrefixes = new[] { "http://localhost:9464/" });
    });

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
