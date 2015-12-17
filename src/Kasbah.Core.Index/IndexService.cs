using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Index.Utils;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index
{
    public class IndexService
    {
        #region Public Constructors

        public IndexService(IIndexProvider indexProvider)
        {
            _indexProvider = indexProvider;

            _handlers = new Dictionary<Type, ICollection<IIndexHandler>>();
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

        public void Store(IDictionary<string, object> value)
            => _indexProvider.Store(value);

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

        readonly IDictionary<Type, ICollection<IIndexHandler>> _handlers;
        readonly IIndexProvider _indexProvider;
        readonly object _lockObj = new object();

        #endregion
    }
}