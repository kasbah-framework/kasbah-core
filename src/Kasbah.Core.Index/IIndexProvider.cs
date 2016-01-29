using System;
using System.Collections.Generic;

namespace Kasbah.Core.Index
{
    public interface IIndexProvider
    {
        #region Public Methods

        void Delete(Guid id);

        IEnumerable<IDictionary<string, object>> Query(object query, int? skip = null, int? take = null, string sort = null);

        void Store(IDictionary<string, object> value);

        #endregion
    }
}