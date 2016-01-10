using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kasbah.Core.Annotations;
using Kasbah.Core.ContentBroker.Events;
using Kasbah.Core.Models;
using Kasbah.Core.ContentTree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.ContentBroker
{
    // TODO: implement caching and event bussing
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

            return MapDictToItem(dict, type);
        }

        public NodeVersion Save<T>(Guid node, Guid version, T item)
            where T : ItemBase
            => Save(node, version, item);

        public NodeVersion Save(Guid node, Guid version, object item)
        {
            var dict = MapItemToDict(item);

            dict["id"] = node;

            var ret = InternalSave(node, version, dict);

            _eventService.Emit<ItemSaved>(new ItemSaved
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

            _eventService.Emit<NodeActiveVersionSet>(new NodeActiveVersionSet
            {
                Payload = node
            });
        }

        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var node = InternalCreateNode(parent, alias, type);

            _eventService.Emit<NodeCreated>(new NodeCreated
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

            return ret.Select(ent => MapDictToItem(ent, type));
        }

        #endregion

        #region Private Methods

        IDictionary<string, object> MapItemToDict(object item)
        {
            if (item == null) { return null; }

            if (item is IDictionary<string, object>) { return item as IDictionary<string, object>; }
            if (item is JObject) { return (item as JObject).ToObject<IDictionary<string, object>>(); }

            var nameResolver = new CamelCasePropertyNamesContractResolver();

            var dict = GetAllProperties(item.GetType())
                // .Where(ent => ent.GetAttributeValue<SystemFieldAttribute, bool>(a => a == null))
                .ToDictionary(ent => nameResolver.GetResolvedPropertyName(ent.Name),
                    ent => ent.GetValue(item, null));

            // TODO: type mapping

            return dict;
        }

        object MapDictToItem(IDictionary<string, object> dict, Type type)
        {
            var item = Activator.CreateInstance(type);
            var nameResolver = new CamelCasePropertyNamesContractResolver();
            var properties = GetAllProperties(type)
                .Where(ent => ent.GetAttributeValue<SystemFieldAttribute, bool>(a => a == null));

            foreach (var prop in properties)
            {
                var name = nameResolver.GetResolvedPropertyName(prop.Name);
                object val;
                if (dict.TryGetValue(name, out val))
                {
                    // TODO: type mapping
                    prop.SetValue(item, val, null);
                }
            }

            if (dict.ContainsKey("id"))
            {
                var id = Guid.Parse(dict["id"] as string);

                // TODO: this needs to be done better.
                var nodeProperty = type.GetProperty("Node");
                nodeProperty?.SetValue(item, GetNode(id), null);
            }

            return item;
        }

        static IEnumerable<PropertyInfo> GetAllProperties(Type type)
        {
            if (type == null) { return Enumerable.Empty<PropertyInfo>(); }

            var info = type.GetTypeInfo();

            return info.DeclaredProperties.Concat(GetAllProperties(info.BaseType));
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