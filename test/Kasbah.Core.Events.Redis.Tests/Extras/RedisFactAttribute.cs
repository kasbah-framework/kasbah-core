using System;
using Xunit;

namespace Kasbah.Core.Events.Redis.Tests
{
    public class RedisFactAttribute : FactAttribute
    {
        #region Public Constructors

        public RedisFactAttribute()
        {
            Skip = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("REDIS")) ? "Redis not available" : null;
        }

        #endregion
    }
}