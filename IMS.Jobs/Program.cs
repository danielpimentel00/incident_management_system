using IMS.GrpcService.Protos;
using IMS.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<IncidentEscalationService>();
builder.Services.AddHostedService<ApiHealthCheckService>();

builder.Services.AddGrpcClient<IncidentService.IncidentServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7286");
});

builder.Services.AddHttpClient("ApiHealthCheck", client =>
{
    client.Timeout = TimeSpan.FromSeconds(10);
});

var host = builder.Build();
host.Run();
