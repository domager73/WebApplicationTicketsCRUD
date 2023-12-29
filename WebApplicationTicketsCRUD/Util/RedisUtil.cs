using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace WebApplicationTicketsCRUD.Util;

public class RedisUtil
{
    private IDistributedCache _cache;

    public RedisUtil(IDistributedCache cache)
    {
        _cache = cache;
    }

    public bool ExistData(string key)
    {
        return _cache.Get(key) == null;
    }

    public T Get<T>(string key)
    {
        byte[] dateFromCache = _cache.Get(key);

        string stringData = Encoding.UTF8.GetString(dateFromCache);

        return JsonSerializer.Deserialize<T>(stringData);
    }
    
    public void Save<T>(string key, T value)
    {
        string toCacheDataString = JsonSerializer.Serialize<T>(value);
        
        byte[] cacheFromDate = Encoding.UTF8.GetBytes(toCacheDataString);

        _cache.Set(key, cacheFromDate);
    }
    
    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}