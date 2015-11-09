using System;
using Xunit;

namespace Kasbah.Core.Events.Redis.Tests
{
    public class RedisFactAttribute : FactAttribute
    {
        public RedisFactAttribute()
        {
            Skip = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("REDIS")) ? "Redis not available" : null;
        }
    }
}