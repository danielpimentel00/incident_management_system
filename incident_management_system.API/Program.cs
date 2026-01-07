using FluentValidation;
using incident_management_system.API.Behaviors;
using incident_management_system.API.Endpoints;
using incident_management_system.API.ExceptionHandlers;
using incident_management_system.API.Extensions;
using incident_management_system.API.Features.Incidents.CreateIncident;
using incident_management_system.API.Health;
using incident_management_system.API.Infrastructure;
using incident_management_system.API.Interfaces;
using incident_management_system.API.Middlewares;
using incident_management_system.API.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<CreateIncidentCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IncidentInMemoryDb>();
builder.Services.AddSingleton<UserInMemoryDb>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseMiddleware<LoggingMiddleware>();

app.MapEndpoints();

app.MapHealthEndpoints();
app.MapIncidentEndpoints();
app.MapUserEndpoints();

app.Run();