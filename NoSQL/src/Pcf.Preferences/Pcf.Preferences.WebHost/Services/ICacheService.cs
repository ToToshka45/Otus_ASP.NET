namespace Pcf.Preferences.WebHost.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;
        Task RemoveAsync<T>( string key, CancellationToken cancellationToken = default );
    }
}
