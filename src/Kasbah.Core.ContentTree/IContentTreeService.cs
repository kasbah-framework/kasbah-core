using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public interface IContentTreeService
    {
        #region Public Methods

        Guid CreateNode<T>(Guid? parent, string alias) where T : ItemBase;

        T GetActiveNodeVersion<T>(Guid id) where T : ItemBase;

        IEnumerable<NodeVersion> GetAllNodeVersions(Guid id);

        Node GetChild(Guid? parent, string alias);

        IEnumerable<Node> GetChildren(Guid? id);

        Node GetNode(Guid id);

        T GetNodeVersion<T>(Guid id, Guid version) where T : ItemBase;

        object GetNodeVersion(Guid id, Guid version, Type type);

        IDictionary<string, object> GetNodeVersion(Guid id, Guid version);

        void MoveNode(Guid id, Guid? parent);

        NodeVersion Save<T>(Guid id, Guid nodeId, T item) where T : ItemBase;

        void SetActiveNodeVersion(Guid id, Guid version);

        #endregion
    }
}