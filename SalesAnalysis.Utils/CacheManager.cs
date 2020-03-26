using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SalesAnalysis.Utils
{
    public class CacheManager<T> : ICacheManager<T>
    {
        private readonly IDistributedCache _cache;
        private static readonly ConcurrentDictionary<string, bool> _keys = new ConcurrentDictionary<string, bool>();
        private static readonly ConcurrentDictionary<string, bool> _keys2 = new ConcurrentDictionary<string, bool>();

        public CacheManager(IDistributedCache cache) => _cache = cache;

        public void Set(string key, object data)
        {
            _cache.SetStringAsync(key, JsonSerializer.Serialize(data));
            _keys.TryAdd(key, true);
        }
        public void Set2(string key, object data)
        {
            _cache.SetStringAsync(key, JsonSerializer.Serialize(data));
            _keys2.TryAdd(key, true);
        }
        public IEnumerable<string> GetAllKeys() => _keys.Keys;
        public IEnumerable<string> GetAllKeys2() => _keys2.Keys;

        public async Task<T> Get(string key) =>
            JsonSerializer.Deserialize<T>(await _cache.GetStringAsync(key));

        public void Remove(string key)
        {
            _cache.Remove(key);
            _keys.TryRemove(key, out _);
        }
        public void Remove2(string key)
        {
            _cache.Remove(key);
            _keys2.TryRemove(key, out _);
        }


        public bool Any() => _keys.Count > 0;

        public bool Any2()
        {
            return _keys2.Count > 0;
        }
    }
}
