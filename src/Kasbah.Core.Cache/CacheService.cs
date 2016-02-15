using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kasbah.Core.Cache
{
    public class CacheService
    {
        #region Public Constructors

        public CacheService()
        {
        }

        public CacheService(IDistributedCache distributedCache = null)
        {
            _distributedCache = distributedCache;
        }

        #endregion

        #region Public Methods

        public object GetOrSet(string key, Type type, Func<object> generator, Func<object, IEnumerable<string>> dependencies = null)
        {
            var ret = Get(key, type)?.Value;

            if (ret == null)
            {
                ret = generator();

                Set(key, ret, dependencies?.Invoke(ret));
            }

            return ret;
        }

        public T GetOrSet<T>(string key, Func<T> generator, Func<T, IEnumerable<string>> dependencies = null)
            where T : class
        {
            var ret = Get(key, typeof(T))?.Value as T;

            if (ret == null)
            {
                ret = generator();

                if (ret != null)
                {
                    Set(key, ret, dependencies?.Invoke(ret));
                }
            }

            return ret;
        }

        /// <summary>
        /// Removes item with specified key from cache
        /// </summary>
        public void Remove(string key)
        {
            var item = Get(key, typeof(object));

            foreach (var dependency in GetDependencies(key))
            {
                Remove(dependency);
            }

            if (_distributedCache != null)
            {
                _distributedCache.Remove(key);
            }
        }

        #endregion

        #region Private Fields

        readonly IDistributedCache _distributedCache;

        #endregion

        #region Private Methods

        CacheEntry Get(string key, Type type)
        {
            if (_distributedCache != null)
            {
                var data = _distributedCache.Get(key);

                if (data == null)
                {
                    return null;
                }

                var entry = JsonConvert.DeserializeObject<IDictionary<string, object>>(Encoding.UTF8.GetString(data, 0, data.Length));

                if (entry == null)
                {
                    return null;
                }

                // TODO: this is messy
                return new CacheEntry
                {
                    Dependencies = (entry["Dependencies"] as JArray).ToObject<IEnumerable<string>>(),
                    Value = (entry["Value"] as JToken).ToObject(type)
                };
            }
            else
            {
                return null;
            }
        }

        IEnumerable<string> GetDependencies(string key)
        {
            var ret = Get($"kasbah:cache_dep:{key}", typeof(IEnumerable<string>));

            return (ret?.Value as IEnumerable<string>) ?? Enumerable.Empty<string>();
        }

        CacheEntry Set(string key, object value, IEnumerable<string> dependencies = null)
        {
            dependencies = dependencies ?? Enumerable.Empty<string>();

            Remove(key);

            var entry = new CacheEntry
            {
                Dependencies = dependencies,
                Value = value
            };

            if (_distributedCache != null)
            {
                foreach (var dependant in dependencies)
                {
                    _distributedCache.Remove(dependant);
                }

                var data = JsonConvert.SerializeObject(entry);

                _distributedCache.Set(key, Encoding.UTF8.GetBytes(data));
            }
            else
            {
                return null;
            }

            if (!key.StartsWith("kasbah:cache_dep:"))
            {
                SetDependencies(key, dependencies);
            }

            return entry;
        }

        void SetDependencies(string key, IEnumerable<string> dependencies)
        {
            if (dependencies != null)
            {
                Set($"kasbah:cache_dep:{key}", dependencies);
            }
        }

        #endregion
    }
}