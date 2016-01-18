using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.ContentBroker.Events;
using Kasbah.Core.ContentBroker.Models;
using Kasbah.Core.ContentTree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;

namespace Kasbah.Core.ContentBroker
{
    public partial class ContentBroker
    {
        #region Public Constructors

        public ContentBroker(ContentTreeService contentTreeService, IndexService indexService, EventService eventService, ILoggerFactory loggerFactory)
        {
            _contentTreeService = contentTreeService;
            _indexService = indexService;
            _eventService = eventService;
            _log = loggerFactory.CreateLogger<ContentBroker>();
        }

        #endregion

        #region Public Methods

        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var node = InternalCreateNode(parent, alias, type);

            _eventService.Emit(new NodeCreated
            {
                Payload = node
            });

            return node;
        }

        public Guid CreateNode(Guid? parent, string alias, Type type)
            => CreateNode(parent, alias, type.AssemblyQualifiedName);

        public Guid CreateNode<T>(Guid? parent, string alias)
            where T : ItemBase
            => CreateNode(parent, alias, typeof(T));

        public Node GetNodeByPath(IEnumerable<string> path)
        {
            var ret = default(Node);

            foreach (var item in path)
            {
                ret = GetChild(ret?.Id, item);
            }

            return ret;
        }

        public T GetNodeVersion<T>(Guid node, Guid version)
            where T : ItemBase
            => GetNodeVersion(node, version, typeof(T)) as T;

        public IDictionary<string, object> GetNodeVersion(Guid node, Guid version)
            => InternalGetNodeVersion(node, version);

        public object GetNodeVersion(Guid node, Guid version, Type type)
        {
            var dict = InternalGetNodeVersion(node, version);

            return TypeHandler.MapDictToItem(type, dict, this);
        }

        public IEnumerable<IDictionary<string, object>> Query(object query, int? take = null, string sort = null)
        {
            // TODO: type mapping

            return _indexService.Query(query, take, sort);
        }

        public IEnumerable<T> Query<T>(object query, int? take = null, string sort = null)
            where T : ItemBase
        {
            return Query(query, typeof(T), take, sort).Cast<T>();
        }

        public IEnumerable<object> Query(object query, Type type, int? take = null, string sort = null)
        {
            var ret = Query(query, take, sort);

            return ret.Select(ent => TypeHandler.MapDictToItem(type, ent, this));
        }

        public NodeVersion Save<T>(Guid node, Guid version, T item)
            where T : ItemBase
            => SaveAnonymous(node, version, item);

        public NodeVersion SaveAnonymous(Guid node, Guid version, object item)
        {
            var dict = TypeHandler.MapItemToDict(item);

            dict["id"] = node;

            var ret = InternalSave(node, version, dict);

            _eventService.Emit(new ItemSaved
            {
                Node = node,
                Version = version,
                Payload = item as ItemBase
            });

            return ret;
        }

        public void SetActiveNodeVersion(Guid node, Guid? version)
        {
            InternalSetActiveNodeVersion(node, version);

            if (!version.HasValue)
            {
                _indexService.Delete(node);
            }
            else
            {
                var nodeItem = GetNode(node);
                var versionItem = InternalGetRawNodeVersion(node, version.Value);
                var dict = GetNodeVersion(node, version.Value);
                dict["id"] = node;
                dict["__nodeType"] = nodeItem.Type;
                dict["__modified"] = versionItem.Modified;
                dict["__created"] = versionItem.Created;

                _indexService.Store(dict);
            }

            _eventService.Emit(new NodeActiveVersionSet
            {
                Payload = node
            });
        }

        #endregion

        #region Private Fields

        readonly ContentTreeService _contentTreeService;
        readonly EventService _eventService;
        readonly IndexService _indexService;
        readonly ILogger _log;

        #endregion
    }
}