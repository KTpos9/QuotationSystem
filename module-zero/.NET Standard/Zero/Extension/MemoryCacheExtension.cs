using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Zero.Extension
{
    public static class MemoryCacheExtension
    {
        public static T GetCache<T>(this IMemoryCache cache, string key, Func<T> getData)
        {
            T obj;

            // Look for cache key.
            if (!cache.TryGetValue(key, out obj))
            {
                // Key not in cache, so get data.
                obj = getData();
            }

            return obj;
        }

        public static async Task<T> GetCacheAsync<T>(this IMemoryCache cache, string key, Func<Task<T>> getData)
        {
            T obj;

            // Look for cache key.
            if (!cache.TryGetValue(key, out obj))
            {
                // Key not in cache, so get data.
                obj = await getData();
            }

            return obj;
        }

        public static void SetCache<T>(this IMemoryCache cache, string key, T obj, TimeSpan cacheTime)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(cacheTime);

            // Save data in cache.
            cache.Set(key, obj, cacheEntryOptions);
        }

        public static void SetCache<T>(this IMemoryCache cache, string key, T obj, int cacheMinutes)
        {
            SetCache(cache, key, obj, TimeSpan.FromMinutes(cacheMinutes));
        }
    }
}