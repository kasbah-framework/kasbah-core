using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public interface IContentTreeService
    {
        #region Public Methods

        Guid CreateNode(Guid? parent, string alias);

        IEnumerable<Tuple<NodeVersion, T>> GetAllNodeVersions<T>(Guid id) where T : ItemBase;

        IEnumerable<Node> GetChildren(Guid? id);

        T GetActiveNodeVersion<T>(Guid id)  where T : ItemBase;

        T GetMostRecentlyCreatedNodeVersion<T>(Guid id) where T : ItemBase;

        void MoveNode(Guid id, Guid? parent);

        NodeVersion Save<T>(Guid id, Guid nodeId, T item) where T : ItemBase;

        #endregion
    }
}
