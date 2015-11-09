using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kasbah.Core.Events.Redis.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Kasbah.Core.Events.Redis
{
    public class RedisEventService : IEventService
    {
        public RedisEventService()
        {
            var config = new ConfigurationOptions
            {
               EndPoints = {
                   Environment.GetEnvironmentVariable("REDIS")
               },
            };

            _redis = ConnectionMultiplexer.Connect(config);

            _subscriptions = new Dictionary<Type, ICollection<Subscription>>();
        }

        public void Emit<T>(T @event) where T : EventBase
        {
            var sub = GetConnection();

            sub.Publish(typeof(T).FullName, JsonConvert.SerializeObject(@event));

            // TODO: not the greatest...
            Thread.Sleep(1000);
        }

        public void Register<T>(IEventHandler handler) where T : EventBase
        {
            var sub = GetConnection();

            Action<RedisChannel, RedisValue> action = (channel, message) =>
            {
                var @event = JsonConvert.DeserializeObject<T>(message);

                handler.HandleEvent<T>(@event);
            };

            var type = typeof(T);
            if (_subscriptions.ContainsKey(type))
            {
                _subscriptions[type].Add(new Subscription(handler, action));
            }
            else
            {
                _subscriptions[type] = new List<Subscription>
                {
                    new Subscription(handler, action)
                };
            }

            sub.Subscribe(typeof(T).FullName, action);
        }

        public void Unregister(IEventHandler handler)
        {
            var sub = GetConnection();


            var subscriptions = _subscriptions
                .Where(ent => ent.Value.Any(ent2 => ent2.Handler == handler))
                .Select(ent => new
                {
                    Type = ent.Key,
                    Actions = ent.Value
                        .Where(ent2 => ent2.Handler == handler)
                        .Select(ent2 => ent2.Action)
                });

            foreach (var subscription in subscriptions.ToList())
            {
                foreach (var actions in subscription.Actions)
                {
                    sub.Unsubscribe(subscription.Type.FullName, actions);
                }
            }

            foreach (var type in subscriptions.Select(ent => ent.Type).Distinct().ToList())
            {
                if (!_subscriptions[type].Any())
                {
                    _subscriptions.Remove(type);
                }
            }
        }

        public void Unregister<T>(IEventHandler handler) where T : EventBase
        {
            var sub = GetConnection();

            var type = typeof(T);

            var subscriptions = _subscriptions[type]
                .Where(ent => ent.Handler == handler);

            foreach (var subscription in subscriptions.ToList())
            {
                sub.Unsubscribe(type.FullName, subscription.Action);

                _subscriptions[type].Remove(subscription);
            }

            if (!_subscriptions[type].Any())
            {
                _subscriptions.Remove(type);
            }
        }

        ISubscriber GetConnection()
        {
            return _redis.GetSubscriber();
        }

        readonly ConnectionMultiplexer _redis;
        readonly IDictionary<Type, ICollection<Subscription>> _subscriptions;
    }
}