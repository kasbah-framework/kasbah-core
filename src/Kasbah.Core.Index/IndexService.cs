using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Events;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.Models;
using Kasbah.Core.Index.Utils;

namespace Kasbah.Core.Index
{
    public class IndexService
    {
        public IndexService(IIndexProvider indexProvider, IEventService eventService, IContentTreeService contentTreeService)
        {
            _indexProvider = indexProvider;
            _eventService = eventService;
            _contentTreeService = contentTreeService;

            _handlers = new Dictionary<Type, ICollection<IIndexHandler>>();

            _contentTreeEventHandler = new ContentTreeEventHandler(this, _contentTreeService);

            eventService.Register<AfterNodeCreated>(_contentTreeEventHandler);
            eventService.Register<AfterItemSaved>(_contentTreeEventHandler);
            eventService.Register<AfterNodeMoved>(_contentTreeEventHandler);
            eventService.Register<NodeActiveVersionSet>(_contentTreeEventHandler);
        }

        public void Register<T>(IIndexHandler handler)
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
                    _handlers[type] = new List<IIndexHandler> { handler };
                }
            }
        }

        public void Unregister(IIndexHandler handler)
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

        public void Unregister<T>(IIndexHandler handler)
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

        public IDictionary<string, object> HandlePreIndex(object item, Type type)
        {
            var indexObject = SerialisationUtil.Serialise(item);

            if (_handlers.ContainsKey(type))
            {
                foreach (var handler in _handlers[type].OrderBy(ent => ent.Priority))
                {
                    var customFields = handler.AddCustomFields(item as ItemBase);
                    foreach (var field in customFields)
                    {
                        indexObject[field.Key] = field.Value;
                    }
                }
            }

            return indexObject;
        }

        public IEnumerable<IDictionary<string, object>> Query(string query)
        {
            return _indexProvider.Query(query);
        }

        readonly IIndexProvider _indexProvider;
        readonly IEventService _eventService;
        readonly IContentTreeService _contentTreeService;
        readonly IDictionary<Type, ICollection<IIndexHandler>> _handlers;
        readonly object _lockObj = new object();
        readonly ContentTreeEventHandler _contentTreeEventHandler;
    }
}