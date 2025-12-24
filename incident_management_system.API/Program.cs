using incident_management_system.API.Endpoints;
using incident_management_system.API.Infrastructure;
using incident_management_system.API.Interfaces;
using incident_management_system.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddSingleton<IncidentInMemoryDb>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIncidentEndpoints();

app.Run();