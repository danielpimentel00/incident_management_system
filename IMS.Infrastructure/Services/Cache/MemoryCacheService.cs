using IMS.Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace IMS.Infrastructure.Services.Cache;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private static readonly ConcurrentDictionary<string, byte> _keysRegistry = new();

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
        {
            options.SetAbsoluteExpiration(expiration.Value);
        }

        options.RegisterPostEvictionCallback((evictedKey, evictedValue, reason, state) =>
        {
            if (evictedKey is string k)
            {
                _keysRegistry.TryRemove(k, out _);
            }
        });

        _cache.Set(key, value, options);
        _keysRegistry.TryAdd(key, 0);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        _keysRegistry.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        var keysToRemove = _keysRegistry.Keys
            .Where(key => key.StartsWith(prefix))
            .ToList();

        foreach (var key in keysToRemove)
        {
            _cache.Remove(key);
            _keysRegistry.TryRemove(key, out _);
        }

        return Task.CompletedTask;
    }
}
