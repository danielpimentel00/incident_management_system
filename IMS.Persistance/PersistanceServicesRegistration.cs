using IMS.Application.Interfaces.Persistance;
using IMS.Persistance.Context;
using IMS.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Persistance;

public static class PersistanceServicesRegistration
{
    public static IServiceCollection AddPersistanceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresDB");

        services.AddDbContext<IncidentDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IIncidentsRepository, IncidentsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
}
