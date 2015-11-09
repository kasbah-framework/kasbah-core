using System;
using StackExchange.Redis;

namespace Kasbah.Core.Events.Redis.Models
{
    class Subscription
    {
        public Subscription(IEventHandler handler, Action<RedisChannel, RedisValue> action)
        {
            Handler = handler;
            Action = action;
        }

        public IEventHandler Handler { get; private set; }

        public Action<RedisChannel, RedisValue> Action { get; private set; }
    }
}