using System;
using System.Collections.Generic;
using System.Linq;

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

        #region Private Methods

        IDictionary<string, object> HandlePreIndex(IDictionary<string, object> item, Type type)
        {
            if (type != null && _handlers.ContainsKey(type))
            {
                foreach (var handler in _handlers[type].OrderBy(ent => ent.Priority))
                {
                    var customFields = handler.AddCustomFields(item);
                    foreach (var field in customFields)
                    {
                        item[field.Key] = field.Value;
                    }
                }
            }

            return item;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes an item from the index with the specified identifier.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        public void Delete(Guid id)
            => _indexProvider.Delete(id);

        /// <see cref="IIndexProvider.Query(object, int?, int?, string)"/>
        public IEnumerable<IDictionary<string, object>> Query(object query, int? skip = null, int? take = null, string sort = null)
            => _indexProvider.Query(query, skip, take, sort);

        /// <summary>
        /// Registers the specified handler.
        /// </summary>
        /// <typeparam name="T">The type of object to handle.</typeparam>
        /// <param name="handler">The handler.</param>
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

        /// <see cref="IIndexProvider.Store(IDictionary{string, object})"/>
        public void Store(IDictionary<string, object> value, Type type)
        {
            value = HandlePreIndex(value, type);

            _indexProvider.Store(value);
        }

        /// <summary>
        /// Unregisters the specified handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
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

        /// <summary>
        /// Unregisters the specified handler from handling items of a specific type.
        /// </summary>
        /// <typeparam name="T">The type the handler should no longer handle.</typeparam>
        /// <param name="handler">The handler.</param>
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