using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public interface IContentTreeService
    {
        Guid CreateNode(Guid? parent, string alias);

        void MoveNode(Guid id, Guid? parent);

        void Save<T>(Guid id, Guid nodeId, T item) where T : ItemBase;

        T GetMostRecentlyCreatedItemVersion<T>(Guid id) where T : ItemBase;

        IEnumerable<Tuple<T, DateTime>> GetAllItemVersions<T>(Guid id) where T : ItemBase;

    }
}
