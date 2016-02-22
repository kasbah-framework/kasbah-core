using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

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
            var ret = Get(key, type);

            if (ret == null)
            {
                ret = generator();

                Set(key, ret, dependencies?.Invoke(ret));
            }

            return ret;
        }

        public async Task<object> GetOrSetAsync(string key, Type type, Func<Task<object>> generator, Func<object, IEnumerable<string>> dependencies = null)
        {
            var ret = Get(key, type);

            if (ret == null)
            {
                ret = await generator();

                Set(key, ret, dependencies?.Invoke(ret));
            }

            return ret;
        }

        public T GetOrSet<T>(string key, Func<T> generator, Func<T, IEnumerable<string>> dependencies = null)
            where T : class
        {
            var ret = Get(key, typeof(T)) as T;

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

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> generator, Func<T, IEnumerable<string>> dependencies = null)
            where T : class
        {
            var ret = Get(key, typeof(T)) as T;

            if (ret == null)
            {
                ret = await generator();

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

            var dependencies = GetDependencies(key);

            if (_distributedCache != null)
            {
                _distributedCache.Remove(key);
            }

            foreach (var dependency in dependencies)
            {
                Remove(dependency);
            }
        }

        #endregion

        #region Private Fields

        readonly IDistributedCache _distributedCache;

        #endregion

        #region Private Methods

        object Get(string key, Type type)
        {
            if (_distributedCache != null)
            {
                var data = _distributedCache.Get(key);

                if (data == null)
                {
                    return null;
                }

                return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data, 0, data.Length), type);
            }
            else
            {
                return null;
            }
        }

        IEnumerable<string> GetDependencies(string key)
        {
            var ret = Get($"kasbah:cache_dep:{key}", typeof(IEnumerable<string>));

            return (ret as IEnumerable<string>) ?? Enumerable.Empty<string>();
        }

        void Set(string key, object value, IEnumerable<string> dependencies = null)
        {
            dependencies = dependencies ?? Enumerable.Empty<string>();

            if (!key.StartsWith("kasbah:cache_dep:"))
            {
                Remove(key);
            }

            if (_distributedCache != null)
            {
                foreach (var dependant in dependencies)
                {
                    Remove(dependant);
                }

                var data = JsonConvert.SerializeObject(value);

                _distributedCache.Set(key, Encoding.UTF8.GetBytes(data));
            }
        }

        void SetDependencies(string key, IEnumerable<string> dependencies)
        {
            if (dependencies != null)
            {
                foreach (var dep in dependencies)
                {
                   var cacheKey = CacheKeys.CacheDependency(dep);
                   var depEntry = Get(cacheKey, typeof(IEnumerable<string>));
                   var depKeys = new[] { key }.AsEnumerable();
                   if (depEntry != null)
                   {
                       depKeys = depKeys.Concat(depEntry as IEnumerable<string>);
                   }

                   depKeys = depKeys.Distinct();

                   Set(cacheKey, depKeys);
                }
            }
        }

        #endregion

        public static class CacheKeys
        {
            public static string CacheDependency(string key)
                => $"kasbah:cache_dep:{key}";
        }
    }
}