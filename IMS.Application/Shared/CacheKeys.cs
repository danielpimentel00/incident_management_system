namespace IMS.Application.Shared;

public static class CacheKeys
{
    public static string IncidentsList(int pageNumber, int pageSize) => $"incidents:list:{pageNumber}:{pageSize}";
}
