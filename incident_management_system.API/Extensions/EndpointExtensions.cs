using incident_management_system.API.Interfaces;

namespace incident_management_system.API.Extensions;

public static class EndpointExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointTypes = typeof(Program).Assembly
            .GetTypes()
            .Where(t =>
                t is { IsAbstract: false, IsInterface: false } &&
                typeof(IEndpoint).IsAssignableFrom(t));

        foreach (var type in endpointTypes)
        {
            var endpoint = (IEndpoint)Activator.CreateInstance(type)!;
            endpoint.MapEndpoints(app);
        }
    }
}
