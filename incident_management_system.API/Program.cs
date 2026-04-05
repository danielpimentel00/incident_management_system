using IMS.Application;
using IMS.Persistance;
using IMS.Infrastructure;
using incident_management_system.API.ExceptionHandlers;
using incident_management_system.API.Extensions;
using incident_management_system.API.Health;
using incident_management_system.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddSerilog(op =>
{
    op.Enrich.FromLogContext();
    op.WriteTo.Console();
    op.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});
builder.Services.AddApplicationServices();
builder.Services.AddPersistanceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<KeyNotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<InvalidOperationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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