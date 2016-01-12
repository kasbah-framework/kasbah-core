using System;
using StackExchange.Redis;

namespace Kasbah.Core.Events.Redis.Models
{
    class Subscription
    {
        #region Public Constructors

        public Subscription(IEventHandler handler, Action<RedisChannel, RedisValue> action)
        {
            Handler = handler;
            Action = action;
        }

        #endregion

        #region Public Properties

        public Action<RedisChannel, RedisValue> Action { get; private set; }
        public IEventHandler Handler { get; private set; }

        #endregion
    }
}