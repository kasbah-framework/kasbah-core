using System;
using System.Collections.Generic;

namespace Kasbah.Core.Index
{
   public interface IIndexProvider
    {
        IEnumerable<IDictionary<string, object>> Query(object query);

        void Store(IDictionary<string, object> value);

        void Delete(Guid id);
    }
}