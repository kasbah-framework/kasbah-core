using System.Collections.Generic;
using Kasbah.Core.Index.Models;
using Kasbah.Core.Events;
using System;
using System.Linq;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree;
using Kasbah.Core.Models;
using Kasbah.Core.Index.Utils;

namespace Kasbah.Core.Index.Solr
{
    public class SolrIndexService : IIndexService, IEventHandler
    {

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

        public IEnumerable<IDictionary<string, object>> Query(string query)
        {
            using (var connection = GetConnection())
            {
                var solrQuery = string.Join(" ", query.Split(' ', '-').Select(ent => $"_text_:{ent}"));
                var response = connection.Select(solrQuery);

                return response.Response.Documents.Select(ent => SolrUtil.ConverFromSolr(ent));
            }
        }

        public void HandleEvent<T>(T @event) where T : EventBase
        {
            // TODO: this is messy.
            if (typeof(NodeEventBase).IsAssignableFrom(typeof(T)))
            {
                IndexNode((@event as NodeEventBase).Data.Id);
            }
            else if (typeof(ItemEventBase).IsAssignableFrom(typeof(T)))
            {
                IndexNode((@event as ItemEventBase).Node);
            }
        }

        public SolrIndexService(IEventService eventService, IContentTreeService contentTreeService)
        {
            _handlers = new Dictionary<Type, ICollection<IIndexHandler>>();

            _eventService = eventService;
            _contentTreeService = contentTreeService;

            eventService.Register<AfterNodeCreated>(this);
            eventService.Register<AfterItemSaved>(this);
            eventService.Register<AfterNodeMoved>(this);
            eventService.Register<NodeActiveVersionSet>(this);
        }

        #region Private Fields

        readonly IDictionary<Type, ICollection<IIndexHandler>> _handlers;
        readonly object _lockObj = new object();
        readonly IEventService _eventService;
        readonly IContentTreeService _contentTreeService;

        #endregion

        #region Private Methods

        void IndexNode(Guid id)
        {
            var node = _contentTreeService.GetNode(id);
            var type = Core.Utils.TypeUtil.TypeFromName(node.Type);

            if (node.ActiveVersionId.HasValue)
            {
                var version = _contentTreeService.GetNodeVersion(id, node.ActiveVersionId.Value, type);

                var indexObject = SerialisationUtil.Serialise(version);

                if (_handlers.ContainsKey(type))
                {
                    foreach (var handler in _handlers[type].OrderBy(ent => ent.Priority))
                    {
                        var customFields = handler.AddCustomFields(version as ItemBase);
                        foreach (var field in customFields)
                        {
                            indexObject[field.Key] = field.Value;
                        }
                    }
                }

                var nodeFields = SerialisationUtil.Serialise(node);
                foreach (var field in nodeFields)
                {
                    indexObject[$"__node_{field.Key}"] = field.Value;
                }

                InsertOrUpdate(id, indexObject);
            }
            else
            {
                Delete(id);
            }
        }

        void InsertOrUpdate(Guid id, IDictionary<string, object> obj)
        {
            using (var connection = GetConnection())
            {
                connection.InsertOrUpdate(id, obj);
            }
        }


        void Delete(Guid id)
        {
            using (var connection = GetConnection())
            {
                connection.Delete(id);
            }
        }

        SolrWebClient GetConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("SOLR");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            return new SolrWebClient(new Uri(connectionString, UriKind.Absolute));
        }

        #endregion
    }
}