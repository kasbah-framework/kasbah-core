using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Cache;
using Kasbah.Core.ContentBroker.Events;
using Kasbah.Core.ContentBroker.Models;
using Kasbah.Core.ContentTree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;

// TODO: unit test the caching strategy to within an inch of its life.

namespace Kasbah.Core.ContentBroker
{
    public partial class ContentBroker
    {
        #region Public Constructors

        public ContentBroker(ContentTreeService contentTreeService, IndexService indexService, EventService eventService, CacheService cacheService, ILoggerFactory loggerFactory)
        {
            _contentTreeService = contentTreeService;
            _indexService = indexService;
            _eventService = eventService;
            _cacheService = cacheService;
            _log = loggerFactory.CreateLogger<ContentBroker>();
        }

        #endregion

        #region Public Methods

        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var node = InternalCreateNode(parent, alias, type);

            _cacheService.Remove($"kasbah:node:{parent}");

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

        public void Delete(Guid id)
        {
            _cacheService.Remove($"kasbah:node:{id}");

            _contentTreeService.Delete(id);
        }
            

        public Node GetChild(Guid? parent, string alias)
        {
            // TODO: dependants
            return _cacheService.GetOrSet<Node>($"kasbah:node:child:{parent}:{alias}",
                () => InternalGetChild(parent, alias));
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            // TODO: dependants
            return _cacheService.GetOrSet<IEnumerable<Node>>($"kasbah:node:children:{id}",
                () => InternalGetChildren(id));
        }

        public Node GetNode(Guid id)
        {
            // TODO: dependants == children
            var cacheKey = $"kasbah:node:{id}";
            return _cacheService.GetOrSet<Node>(cacheKey,
                () => InternalGetNode(id),
                (node) =>
                {
                    var ret = new[]
                    {
                        $"kasbah:node:children:{node.Parent}",
                        $"kasbah:node:child:{node.Parent}:{node.Alias}"
                    };

                    return ret.Concat(GetChildren(node.Id).SelectMany(child => new[] { $"kasbah:node:{child.Id}", $"kasbah:noder:child:{node.Id}:{child.Alias}" }));
                });
        }

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
            // TODO: dependants
            var cacheKey = $"kasbah:node_version:{node}:{version}";
            return _cacheService.GetOrSet(cacheKey, type, () =>
            {
                var dict = InternalGetNodeVersion(node, version);

                return TypeHandler.MapDictToItem(type, dict, this);
            },
            (nodeVersion) =>
            {
                return new[]
                {
                    $"kasbah:node:{node}"
                };
            });
        }

        public IEnumerable<IDictionary<string, object>> Query(object query, int? skip = null, int? take = null, string sort = null)
            => _indexService.Query(query, skip, take, sort);

        public IEnumerable<T> Query<T>(object query, int? skip = null,int? take = null, string sort = null)
            where T : ItemBase
            => Query(query, typeof(T), skip, take, sort).Cast<T>();

        public IEnumerable<object> Query(object query, Type type, int? skip = null, int? take = null, string sort = null)
        {
            // TODO: dependants
            var cacheKey = $"kasbah:query:{query.GetHashCode()}:{type}:{take}:{sort}";
            return _cacheService.GetOrSet(cacheKey, () =>
            {
                var ret = Query(query, skip, take, sort);

                return ret.Select(ent => TypeHandler.MapDictToItem(type, ent, this));
            });
        }

        public NodeVersion Save<T>(Guid node, Guid version, T item)
            where T : ItemBase
            => SaveAnonymous(node, version, item);

        public NodeVersion SaveAnonymous(Guid node, Guid version, object item)
        {
            _cacheService.Remove($"kasbah:node_version:{node}:{version}");

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
            var nodeItem = GetNode(node);
            _cacheService.Remove($"kasbah:node:{node}");

            InternalSetActiveNodeVersion(node, version);

            if (!version.HasValue)
            {
                _indexService.Delete(node);
            }
            else
            {
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

        readonly CacheService _cacheService;
        readonly ContentTreeService _contentTreeService;
        readonly EventService _eventService;
        readonly IndexService _indexService;
        readonly ILogger _log;

        #endregion
    }
}