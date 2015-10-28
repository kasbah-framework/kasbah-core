using System;
using System.Collections.Generic;

namespace Kasbah.Core.Events
{
    public class EventService
    {
        readonly IDictionary<Type, ICollection<IEventHandler>> _handlers;
        readonly object _lockObj = new object();

        public EventService()
        {
            _handlers = new Dictionary<Type, ICollection<IEventHandler>>();
        }

        public void Register<T>(IEventHandler handler)
            where T : EventBase
        {
            lock (_lockObj)
            {
                var type = typeof(T);
                if (_handlers.ContainsKey(type))
                {
                    _handlers[type].Add(handler);
                }
                else
                {
                    _handlers[type] = new List<IEventHandler>
                {
                    handler
                };
                }
            }
        }

        public void Emit<T>(T @event)
            where T : EventBase
        {
            lock (_lockObj)
            {
                var type = typeof(T);
                if (_handlers.ContainsKey(type))
                {
                    foreach (var handler in _handlers[type])
                    {
                        handler.HandleEvent<T>(@event);
                    }
                }
            }
        }
    }
}