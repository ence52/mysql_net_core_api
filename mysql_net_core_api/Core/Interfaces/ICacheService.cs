namespace mysql_net_core_api.Core.Interfaces
{
    public interface ICacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expire = null);
        Task<T?> GetAsync<T> (string key);
        Task RemoveAsync(string key);
    }
}
