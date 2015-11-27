using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index
{
    public interface IIndexProvider
    {
        #region Public Methods

        void Delete(Guid id);

        IEnumerable<IDictionary<string, object>> Query(object query);

        IEnumerable<T> Query<T>(object query)
            where T : ItemBase, new();

        void Store(IDictionary<string, object> value);

        #endregion
    }
}