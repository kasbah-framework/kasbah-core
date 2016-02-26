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

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        public CacheService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache if present.</param>
        public CacheService(IDistributedCache distributedCache = null)
        {
            _distributedCache = distributedCache;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an item from the cache if it exists, if not, calls the generator function to store the value.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="type">The type of object stored in the cache.</param>
        /// <param name="generator">The function used to generate the value to be stored.</param>
        /// <param name="dependencies">
        /// The cache entries that depend on this entry.  If this entry (identified by <paramref name="key"/>
        /// change) then these entries will be invalidated.
        /// </param>
        /// <returns>The object stored in the cache.</returns>
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

        /// <see cref="GetOrSet(string, Type, Func{object}, Func{object, IEnumerable{string}})"/>
        public T GetOrSet<T>(string key, Func<T> generator, Func<T, IEnumerable<string>> dependencies = null)
            where T : class
            => GetOrSet(key, typeof(T), generator, dependencies: (o) => dependencies?.Invoke(o as T)) as T;

        /// <see cref="GetOrSet(string, Type, Func{object}, Func{object, IEnumerable{string}})"/>
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

        /// <see cref="GetOrSet(string, Type, Func{object}, Func{object, IEnumerable{string}})"/>
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> generator, Func<T, IEnumerable<string>> dependencies = null)
            where T : class
            => await GetOrSetAsync(key, typeof(T), async () => await generator() as T, (o) => dependencies?.Invoke(o as T)) as T;

        /// <summary>
        /// Removes the entry specified by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key identifier for the cache entry.</param>
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

        #region Public Classes

        public static class CacheKeys
        {
            #region Public Methods

            public static string CacheDependency(string key)
                => $"kasbah:cache_dep:{key}";

            #endregion
        }

        #endregion
    }
}