using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.Events;
using Kasbah.Core.Index.Utils;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index
{
    public class IndexService
    {
        #region Public Constructors

        public IndexService(IIndexProvider indexProvider, IEventService eventService, ContentTreeService contentTreeService)
        {
            _indexProvider = indexProvider;
            _eventService = eventService;
            _contentTreeService = contentTreeService;

            _handlers = new Dictionary<Type, ICollection<IIndexHandler>>();

            _contentTreeEventHandler = new ContentTreeEventHandler(this, _indexProvider, _contentTreeService);

            eventService.Register<AfterItemSaved>(_contentTreeEventHandler);
            eventService.Register<NodeActiveVersionSet>(_contentTreeEventHandler);
        }

        #endregion

        #region Public Methods

        public IDictionary<string, object> HandlePreIndex(object item, Type type)
        {
            var indexObject = default(IDictionary<string, object>);
            if (item is IDictionary<string, object>)
            {
                indexObject = item as IDictionary<string, object>;
            }
            else
            {
                indexObject = SerialisationUtil.Serialise(item);
            }

            if (type != null && _handlers.ContainsKey(type))
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

        public void Noop()
        {
        }

        public IEnumerable<IDictionary<string, object>> Query(object query)
        {
            return _indexProvider.Query(query);
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

        #endregion

        #region Private Fields

        readonly ContentTreeEventHandler _contentTreeEventHandler;
        readonly ContentTreeService _contentTreeService;
        readonly IEventService _eventService;
        readonly IDictionary<Type, ICollection<IIndexHandler>> _handlers;
        readonly IIndexProvider _indexProvider;
        readonly object _lockObj = new object();

        #endregion
    }
}