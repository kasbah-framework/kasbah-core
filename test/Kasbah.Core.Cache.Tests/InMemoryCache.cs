using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Kasbah.Core.Cache.Tests
{
    public class InMemoryCache : IDistributedCache
    {
        static IDictionary<string, byte[]> _cache = new Dictionary<string, byte[]>();

        public void Connect()
        {

        }

        public async Task ConnectAsync()
        {
            await Task.Delay(0);
        }

        public byte[] Get(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return null;
        }

        public async Task<byte[]> GetAsync(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return await Task.FromResult(_cache[key]);
            }
            return null;
        }

        public void Refresh(string key)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            Remove(key);
            await Task.Delay(0);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _cache[key] = value;
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            Set(key, value, options);
            await Task.Delay(0);
        }
    }
}