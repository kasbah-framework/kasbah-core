using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentBroker
{
    public partial class ContentBroker
    {
        #region Public Constructors

        public ContentBroker(ContentTreeService contentTreeService, IndexService indexService, EventService eventService)
        {
            _contentTreeService = contentTreeService;
            _indexService = indexService;
            _eventService = eventService;
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

            var item = MapDictToItem(dict, type);

            return item;
        }

        public NodeVersion Save<T>(Guid node, Guid version, T item)
            where T : ItemBase
            => Save(node, version, item);

        public NodeVersion Save(Guid node, Guid version, object item)
        {
            var dict = MapItemToDict(item);

            return InternalSave(node, version, dict);
        }

        public void SetActiveNodeVersion(Guid node, Guid? version)
        {
            // if version == null, clear index/cache
            // if version != null, write/overwrite index/cache

            InternalSetActiveNodeVersion(node, version);
        }

        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            return InternalCreateNode(parent, alias, type);
        }

        public Guid CreateNode(Guid? parent, string alias, Type type)
            => CreateNode(parent, alias, type.AssemblyQualifiedName);

        public Guid CreateNode<T>(Guid? parent, string alias)
            where T : ItemBase
            => CreateNode(parent, alias, typeof(T));

        #endregion

        #region Private Methods

        IDictionary<string, object> MapItemToDict(object item)
        {
            if (item == null) { return null; }

            var dict = GetAllProperties(item.GetType())
                .ToDictionary(ent => ent.Name, ent => ent.GetValue(item, null));

            // TODO: type mapping

            return dict;
        }

        object MapDictToItem(IDictionary<string, object> dict, Type type)
        {
            var item = Activator.CreateInstance(type);

            foreach (var prop in GetAllProperties(type))
            {
                object val;
                if (dict.TryGetValue(prop.Name, out val))
                {
                    // TODO: type mapping
                    prop.SetValue(item, val, null);
                }
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

        #endregion
    }
}