using System;
using System.Collections.Generic;
using Kasbah.Core.Models;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Utils;

namespace Kasbah.Core.ContentBroker
{
    // TODO: implement caching and event bussing
    public partial class ContentBroker
    {
        #region Public Methods

        public Guid CreateNode<T>(Guid? parent, string alias) where T : ItemBase
            => _contentTreeService.CreateNode(parent, alias, typeof(T));

        public Guid CreateNode(Guid? parent, string alias, Type type)
            => _contentTreeService.CreateNode(parent, alias, type.AssemblyQualifiedName);

        public Guid CreateNode(Guid? parent, string alias, string type)
            => _contentTreeService.CreateNode(parent, alias, type);

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
            => _contentTreeService.GetAllNodeVersions(id);

        public Node GetChild(Guid? parent, string alias)
            => _contentTreeService.GetChild(parent, alias);

        public IEnumerable<Node> GetChildren(Guid? id)
            => _contentTreeService.GetChildren(id);

        public Node GetNode(Guid id)
            => _contentTreeService.GetNode(id);

        public T GetNodeVersion<T>(Guid id, Guid version) where T : ItemBase
            => _contentTreeService.GetNodeVersion(id, version, typeof(T)) as T;

        public object GetNodeVersion(Guid id, Guid version, Type type)
            => _contentTreeService.GetNodeVersion(id, version, type);

        public object GetNodeVersion(Guid id, Guid version, string type)
            => _contentTreeService.GetNodeVersion(id, version, TypeUtil.TypeFromName(type));

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
            => _contentTreeService.GetNodeVersion(id, version);

        public void MoveNode(Guid id, Guid? parent)
            => _contentTreeService.MoveNode(id, parent);

        public NodeVersion Save<T>(Guid id, Guid node, T item) where T : ItemBase
            => _contentTreeService.Save(id, node, (object)item);

        public NodeVersion Save(Guid id, Guid node, object item)
            => _contentTreeService.Save(id, node, item);

        public void SetActiveNodeVersion(Guid id, Guid? version)
            => _contentTreeService.SetActiveNodeVersion(id, version);

        public Guid GetOrCreate(Guid? parent, string alias, Type type)
            => _contentTreeService.GetOrCreate(parent, alias, type);

        public Guid GetOrCreate<T>(Guid? parent, string alias) where T : ItemBase
            => GetOrCreate(parent, alias, typeof(T));

        #endregion
    }
}