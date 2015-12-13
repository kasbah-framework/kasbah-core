using System;
using System.Collections.Generic;
using System.Linq;

namespace Kasbah.Core.Events.InProc
{
    public class InProcEventBusProvider : IEventBusProvider
    {
        #region Public Constructors

        public InProcEventBusProvider()
        {
            _handlers = new Dictionary<Type, ICollection<IEventHandler>>();
        }

        #endregion

        #region Public Methods

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
                    _handlers[type] = new List<IEventHandler> { handler };
                }
            }
        }

        public void Unregister(IEventHandler handler)
        {
            lock (_lockObj)
            {
                foreach (var kvp in _handlers.Where(ent => ent.Value.Contains(handler)).ToList())
                {
                    kvp.Value.Remove(handler);
                    if (kvp.Value.Count == 0)
                    {
                        _handlers.Remove(kvp.Key);
                    }
                }
            }
        }

        public void Unregister<T>(IEventHandler handler)
            where T : EventBase
        {
            lock (_lockObj)
            {
                var type = typeof(T);
                if (_handlers.ContainsKey(type) && _handlers[type].Contains(handler))
                {
                    _handlers[type].Remove(handler);
                    if (_handlers[type].Count == 0)
                    {
                        _handlers.Remove(type);
                    }
                }
            }
        }

        #endregion

        #region Private Fields

        readonly IDictionary<Type, ICollection<IEventHandler>> _handlers;
        readonly object _lockObj = new object();

        #endregion
    }
}