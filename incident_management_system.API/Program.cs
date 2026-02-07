using FluentValidation;
using incident_management_system.API.Behaviors;
using incident_management_system.API.ExceptionHandlers;
using incident_management_system.API.Extensions;
using incident_management_system.API.Features.Incidents.CreateIncident;
using incident_management_system.API.Health;
using incident_management_system.API.Infrastructure;
using incident_management_system.API.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IncidentDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDB"));
});

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<CreateIncidentCommandValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["JWT:Authority"];
        options.Audience = builder.Configuration["JWT:Audience"];
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.UseMiddleware<LoggingMiddleware>();

app.MapEndpoints();

app.MapHealthEndpoints();

app.Run();