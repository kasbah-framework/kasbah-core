using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kasbah.Core.Cache
{
    public class CacheService
    {
        public CacheService(IMemoryCache memoryCache = null, IDistributedCache distributedCache = null)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        public CacheEntry Get(string key, Type type)
        {
            if (_distributedCache != null)
            {
                var data = _distributedCache.Get(key);

                var entry = JsonConvert.DeserializeObject<IDictionary<string, object>>(Encoding.UTF8.GetString(data, 0, data.Length));

                // TODO: this is messy
                return new CacheEntry
                {
                    Dependants = (entry["Dependants"] as JArray).ToObject<IEnumerable<string>>(),
                    Value = (entry["Value"] as JObject).ToObject(type)
                };
            }
            else if (_memoryCache != null)
            {
                return _memoryCache.Get(key) as CacheEntry;
            }
            else
            {
                return null;
            }
        }

        public CacheEntry Set(string key, object value, IEnumerable<string> dependants = null)
        {
            Remove(key);

            var entry = new CacheEntry
            {
                Dependants = dependants,
                Value = value
            };

            if (_distributedCache != null)
            {
                foreach (var dependant in dependants)
                {
                    _distributedCache.Remove(dependant);
                }

                var data = JsonConvert.SerializeObject(entry);

                _distributedCache.Set(key, Encoding.UTF8.GetBytes(data));
            }
            else if (_memoryCache != null)
            {
                foreach (var dependant in dependants)
                {
                    _memoryCache.Remove(dependant);
                }

                _memoryCache.Set(key, entry);
            }
            else
            {
                return null;
            }

            return entry;
        }

        public object GetOrSet(string key, Type type, Func<object> generator, Func<IEnumerable<string>> dependants = null)
        {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            if (type == null) { throw new ArgumentNullException(nameof(type)); }
            if (generator == null) { throw new ArgumentNullException(nameof(generator)); }

            var ret = Get(key, type)?.Value;

            if (ret == null)
            {
                ret = generator();

                Set(key, ret, dependants());
            }

            return ret;
        }

        public T GetOrSet<T>(string key, Func<T> generator, Func<IEnumerable<string>> dependants = null)
            where T : class
            => GetOrSet(key, typeof(T), generator, dependants) as T;

        /// <summary>
        /// Removes item with specified key from cache
        /// </summary>
        public void Remove(string key)
        {
            var item = Get(key, typeof(object));

            if (_distributedCache != null)
            {
                _distributedCache.Remove(key);
            }
            else if (_memoryCache != null)
            {
                _memoryCache.Remove(key);
            }

            if (item != null && item.Dependants != null)
            {
                foreach (var dependant in item.Dependants)
                {
                    Remove(dependant);
                }
            }
        }

        readonly IMemoryCache _memoryCache;
        readonly IDistributedCache _distributedCache;
    }
}