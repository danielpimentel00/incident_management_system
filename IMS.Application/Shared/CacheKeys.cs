namespace IMS.Application.Shared;

public static class CacheKeys
{
    public const string OpenIncidents = "incidents:open";
    public const string IncidentsListPrefix = "incidents:list";

    public static string IncidentsList(int pageNumber, int pageSize) => $"{IncidentsListPrefix}:{pageNumber}:{pageSize}";
    public static string IncidentById(int id) => $"incident:{id}";
}
