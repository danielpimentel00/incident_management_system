using incident_management_system.API.Endpoints;
using incident_management_system.API.ExceptionHandlers;
using incident_management_system.API.Infrastructure;
using incident_management_system.API.Interfaces;
using incident_management_system.API.Middlewares;
using incident_management_system.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddSingleton<IncidentInMemoryDb>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseMiddleware<LoggingMiddleware>();

app.MapIncidentEndpoints();

app.Run();