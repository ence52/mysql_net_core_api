using mysql_net_core_api.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace mysql_net_core_api.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger)
        {
            _cacheDb = redis.GetDatabase();
            _logger = logger;
        }
        public async Task<T?> GetAsync<T>(string key)
        {

            try
            {
                var data = await _cacheDb.StringGetAsync(key);
                if (!data.HasValue)
                {
                    _logger.LogWarning($"Cache miss: {key}");
                    return default;
                }
                _logger.LogInformation($"Cache hit: {key}");
                return JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache get error for key: {key}");
                throw;
            }


        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cacheDb.KeyDeleteAsync(key);
                _logger.LogInformation($"Cache removed: {key}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to remove {key}", e);
                throw;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expire = null)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(value);
                await _cacheDb.StringSetAsync(key, jsonData, expire);
                _logger.LogInformation($"Cache set: {key}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache set error for key :{key}");
                throw;
            }
        }
    }
}
