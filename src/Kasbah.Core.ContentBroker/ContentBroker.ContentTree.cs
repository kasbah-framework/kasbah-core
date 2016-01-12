using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentBroker
{
    public partial class ContentBroker
    {
        #region Private Methods

        Guid InternalCreateNode(Guid? parent, string alias, string type)
             => _contentTreeService.CreateNode(parent, alias, type);

        IDictionary<string, object> InternalGetNodeVersion(Guid id, Guid version)
            => _contentTreeService.GetNodeVersion(id, version);

        NodeVersion InternalGetRawNodeVersion(Guid id, Guid version)
            => _contentTreeService.GetRawNodeVersion(id, version);

        NodeVersion InternalSave(Guid node, Guid version, IDictionary<string, object> item)
                             => _contentTreeService.Save(version, node, item);

        void InternalSetActiveNodeVersion(Guid id, Guid? version)
              => _contentTreeService.SetActiveNodeVersion(id, version);

        #endregion

        #region Public Methods

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
            => _contentTreeService.GetAllNodeVersions(id);

        public Node GetChild(Guid? parent, string alias)
            => _contentTreeService.GetChild(parent, alias);

        public IEnumerable<Node> GetChildren(Guid? id)
            => _contentTreeService.GetChildren(id);

        public Node GetNode(Guid id)
            => _contentTreeService.GetNode(id);

        public Guid GetOrCreate(Guid? parent, string alias, Type type)
            => _contentTreeService.GetOrCreate(parent, alias, type);

        public Guid GetOrCreate<T>(Guid? parent, string alias) where T : ItemBase
            => GetOrCreate(parent, alias, typeof(T));

        public void MoveNode(Guid id, Guid? parent)
            => _contentTreeService.MoveNode(id, parent);

        #endregion
    }
}