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
using Kasbah.Core.Utils;
using Microsoft.Extensions.Logging;

// TODO: unit test the caching strategy to within an inch of its life.

namespace Kasbah.Core.ContentBroker
{
    public partial class ContentBroker
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentBroker"/> class.
        /// </summary>
        /// <param name="contentTreeService">The content tree service.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="eventService">The event service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
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

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="parent">The parent node identifier.</param>
        /// <param name="alias">The alias identify the node.</param>
        /// <param name="type">The type of node being created.</param>
        /// <returns>The identifier for the created node.</returns>
        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var node = InternalCreateNode(parent, alias, type);

            _cacheService.Remove(CacheKeys.Children(parent));

            _eventService.Emit(new NodeCreated
            {
                Payload = node
            });

            return node;
        }

        /// <see cref="CreateNode(Guid?, string, string)"/>
        public Guid CreateNode(Guid? parent, string alias, Type type)
            => CreateNode(parent, alias, type.AssemblyQualifiedName);

        /// <see cref="CreateNode(Guid?, string, string)"/>
        public Guid CreateNode<T>(Guid? parent, string alias)
            where T : ItemBase
            => CreateNode(parent, alias, typeof(T));

        /// <summary>
        /// Deletes the specified node.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        public void Delete(Guid id)
        {
            var node = GetNode(id);

            _cacheService.Remove(CacheKeys.Node(id));
            _cacheService.Remove(CacheKeys.Children(node.Parent));
            _cacheService.Remove(CacheKeys.Child(node.Parent, node.Alias));

            _contentTreeService.Delete(id);
            _indexService.Delete(id);
        }

        /// <summary>
        /// Gets the child node under <paramref name="parent"/> with <paramref name="alias"/>.
        /// </summary>
        /// <param name="parent">The parent node identifier.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The requested child node if it exists, otherwise null.</returns>
        public Node GetChild(Guid? parent, string alias)
        {
            return _cacheService.GetOrSet(CacheKeys.Child(parent, alias),
                () => InternalGetChild(parent, alias),
                (node) => node.Parent.HasValue ? new[] { CacheKeys.Node(node.Parent.Value) } : null);
        }

        /// <summary>
        /// Gets the children nodes under the node with identifier <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <returns>All children nodes under the specified node.</returns>
        public IEnumerable<Node> GetChildren(Guid? id)
        {
            return _cacheService.GetOrSet(CacheKeys.Children(id),
                () => InternalGetChildren(id),
                (children) => children.Select(ent => CacheKeys.Node(ent.Id)));
        }

        /// <summary>
        /// Gets a single node with identifier.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <returns>The requested node if it exists, otherwise null.</returns>
        public Node GetNode(Guid id)
        {
            return _cacheService.GetOrSet(CacheKeys.Node(id),
                () => InternalGetNode(id));
        }

        /// <summary>
        /// Gets the node by alias path.
        /// </summary>
        /// <param name="path">The alias path.</param>
        /// <returns>The node at the given path if it exists, otherwise null.</returns>
        public Node GetNodeByPath(IEnumerable<string> path)
        {
            var ret = default(Node);

            foreach (var item in path)
            {
                ret = GetChild(ret?.Id, item);
            }

            return ret;
        }

        /// <see cref="GetNodeVersion(Guid, Guid, Type)"/>
        public T GetNodeVersion<T>(Guid node, Guid version)
            where T : ItemBase
            => GetNodeVersion(node, version, typeof(T)) as T;

        /// <see cref="GetNodeVersion(Guid, Guid, Type)"/>
        public IDictionary<string, object> GetNodeVersion(Guid node, Guid version)
            => InternalGetNodeVersion(node, version);

        /// <summary>
        /// Gets the specified node version.
        /// </summary>
        /// <param name="node">The node identifier.</param>
        /// <param name="version">The version identifier.</param>
        /// <param name="type">The type the version should be mapped to.</param>
        /// <returns>The strongly typed node version.</returns>
        public object GetNodeVersion(Guid node, Guid version, Type type)
        {
            // TODO: dependants
            var dict = _cacheService.GetOrSet(CacheKeys.NodeVersion(node, version),
                () => InternalGetNodeVersion(node, version),
                (nodeVersion) => new[] { CacheKeys.Node(node) });

            return TypeHandler.MapDictToItem(type, dict, this);
        }

        /// <see cref="Query(object, Type, int?, int?, string)"/>
        public IEnumerable<IDictionary<string, object>> Query(object query, int? skip = null, int? take = null, string sort = null)
            => _indexService.Query(query, skip, take, sort);

        /// <see cref="Query(object, Type, int?, int?, string)"/>
        public IEnumerable<T> Query<T>(object query, int? skip = null, int? take = null, string sort = null)
            where T : ItemBase
            => Query(query, typeof(T), skip, take, sort).Cast<T>();

        /// <summary>
        /// Queries the node index.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="type">The type of content to return.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to take.</param>
        /// <param name="sort">The sort order.</param>
        /// <returns>A list of objects that match the query.</returns>
        public IEnumerable<object> Query(object query, Type type, int? skip = null, int? take = null, string sort = null)
        {
            var ret = Query(query, skip, take, sort);

            return ret.Select(ent => TypeHandler.MapDictToItem(type, ent, this));
        }

        /// <see cref="SaveAnonymous(Guid, Guid, object)"/>
        public NodeVersion Save<T>(Guid node, Guid version, T item)
            where T : ItemBase
            => SaveAnonymous(node, version, item);

        /// <summary>
        /// Saves the node version.
        /// </summary>
        /// <param name="node">The node identifier.</param>
        /// <param name="version">The version identifier.</param>
        /// <param name="item">The item data.</param>
        /// <returns>
        /// The node version that has been saved.  If the version identifier doesn't
        /// exist it will be created, otherwise it will be updated.
        /// </returns>
        public NodeVersion SaveAnonymous(Guid node, Guid version, object item)
        {
            _cacheService.Remove(CacheKeys.NodeVersion(node, version));

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

        /// <summary>
        /// Sets the active node version.
        /// </summary>
        /// <param name="node">The node identifier.</param>
        /// <param name="version">The version identifier.</param>
        public void SetActiveNodeVersion(Guid node, Guid? version)
        {
            InternalSetActiveNodeVersion(node, version);

            _cacheService.Remove(CacheKeys.Node(node));

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

                _indexService.Store(dict, TypeUtil.TypeFromName(nodeItem.Type));
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

        #region Public Classes

        public static class CacheKeys
        {
            #region Public Methods

            public static string Child(Guid? id, string alias)
                => $"{Prefix}:node_child:{id}:{alias}";

            public static string Children(Guid? id)
                => $"{Prefix}:node_children:{id}";

            public static string Node(Guid id)
                => $"{Prefix}:node:{id}";

            public static string NodeVersion(Guid node, Guid version)
                => $"{Prefix}:node_version:{node}:{version}";

            #endregion

            #region Private Fields

            const string Prefix = "kasbah";

            #endregion
        }

        #endregion
    }
}