using Microsoft.Extensions.Caching.Memory;

namespace ShippingCostCalculator.Helpers
{
    public static class MemoryCacheExtensions
    {
        private static readonly MemoryCacheEntryOptions _defaultCacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1), // Expires in 1 minute
            SlidingExpiration = TimeSpan.FromSeconds(30) // Resets expiration if accessed within 30 seconds
        };

        public static void SetWithDefaults<T>(this IMemoryCache cache, string key, T value)
        {
            cache.Set(key, value, _defaultCacheEntryOptions);
        }
    }
}
