namespace Schedule.Application; 

public static class CacheKeyProvider
{
    public static string GetCacheKey(string dbPrefix, Guid id) => $"{dbPrefix}:getShceduleById:{id}";

    public static string GetCacheKey(string dbPrefix, int year, int month) => $"{dbPrefix}:getShceduleByMonth:{year}:{month}";
}
