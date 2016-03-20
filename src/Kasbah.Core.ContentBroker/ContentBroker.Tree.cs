using System;
using System.Collections.Generic;
using Kasbah.Core.ContentBroker.Models;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentBroker
{
    public partial class ContentBroker
    {
        #region Private Methods

        Guid InternalCreateNode(Guid? parent, string alias, string type)
            => _treeService.CreateNode(parent, alias, type);

        Node InternalGetChild(Guid? parent, string alias)
            => _treeService.GetChild(parent, alias);

        IEnumerable<Node> InternalGetChildren(Guid? id)
            => _treeService.GetChildren(id);

        Node InternalGetNode(Guid id)
            => _treeService.GetNode(id);

        IDictionary<string, object> InternalGetNodeVersion(Guid id, Guid version)
                                    => _treeService.GetNodeVersion(id, version);

        NodeVersion InternalGetRawNodeVersion(Guid id, Guid version)
            => _treeService.GetRawNodeVersion(id, version);

        NodeVersion InternalSave(Guid node, Guid version, IDictionary<string, object> item)
            => _treeService.Save(version, node, item);

        void InternalSetActiveNodeVersion(Guid id, Guid? version)
            => _treeService.SetActiveNodeVersion(id, version);

        #endregion

        #region Public Methods

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
            => _treeService.GetAllNodeVersions(id);

        public Guid GetOrCreate(Guid? parent, string alias, Type type)
            => _treeService.GetOrCreate(parent, alias, type);

        public Guid GetOrCreate<T>(Guid? parent, string alias) where T : ItemBase
            => GetOrCreate(parent, alias, typeof(T));

        public void MoveNode(Guid id, Guid? parent)
            => _treeService.MoveNode(id, parent);

        #endregion
    }
}