using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public interface IContentTreeProvider
    {
        #region Public Methods

        void CreateNode(Guid id, Guid? parent, string alias, string type);

        T GetActiveNodeVersion<T>(Guid id) where T : ItemBase;

        IEnumerable<NodeVersion> GetAllNodeVersions(Guid id);

        Node GetChild(Guid? parent, string alias);

        IEnumerable<Node> GetChildren(Guid? id);

        Node GetNode(Guid id);

        object GetNodeVersion(Guid id, Guid version, Type type);

        IDictionary<string, object> GetNodeVersion(Guid id, Guid version);

        void MoveNode(Guid id, Guid? parent);

        NodeVersion Save(Guid id, Guid node, object item);

        void SetActiveNodeVersion(Guid id, Guid version);

        #endregion
    }
}