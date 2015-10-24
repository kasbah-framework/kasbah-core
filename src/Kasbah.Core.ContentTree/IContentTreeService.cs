using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public interface IContentTreeService
    {
        void Save<T>(Guid id, T item) where T : ItemBase;

        T GetMostRecentlyCreatedItemVersion<T>(Guid id);

        IEnumerable<Tuple<T, DateTime>> GetAllItemVersions<T>(Guid id);

    }
}
