using IMS.GrpcService.Protos;
using IMS.Jobs.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<IncidentEscalationService>();

builder.Services.AddGrpcClient<IncidentService.IncidentServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7286");
});

var host = builder.Build();
host.Run();
