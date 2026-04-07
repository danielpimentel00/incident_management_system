using IMS.Application;
using IMS.GrpcService.Services;
using IMS.Infrastructure;
using IMS.Persistance;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("IMS.GrpcService"));
        tracing.AddAspNetCoreInstrumentation();
        tracing.AddConsoleExporter();
    });

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<IncidentGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
