namespace Schedule.Application; 

public static class CacheKeyProvider
{
    public static string GetCacheKey(string dbPrefix, Guid userId, int year, int month) => $"{dbPrefix}:getShceduleByUserIdByMonth:{userId}:{year}:{month}";
}
