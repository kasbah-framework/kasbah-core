using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index
{
   public interface IIndexProvider
    {
        IEnumerable<IDictionary<string, object>> Query(object query);

        IEnumerable<T> Query<T>(object query)
            where T : ItemBase, new();

        void Store(IDictionary<string, object> value);

        void Delete(Guid id);
    }
}