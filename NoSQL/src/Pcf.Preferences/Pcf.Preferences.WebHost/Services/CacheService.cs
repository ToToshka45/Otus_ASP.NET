using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Pcf.Preferences.WebHost.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService( IDistributedCache distributedCache )
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>( string key, CancellationToken cancellationToken = default ) where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync( key, cancellationToken );

            if ( cachedValue is null )
            {
                return null;
            }

            T? value = JsonConvert.DeserializeObject<T>( cachedValue );

            return value;
        }

        public async Task SetAsync<T>( string key, T value, CancellationToken cancellationToken = default ) where T : class
        {
            string cacheValues = JsonConvert.SerializeObject( value );

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds( 300 )
            };

            await _distributedCache.SetStringAsync( key, cacheValues, options, cancellationToken );
        }

        public async Task RemoveAsync<T>( string key, CancellationToken cancellationToken = default )
        {
            await _distributedCache.RemoveAsync( key, cancellationToken );
        }
    }
}
