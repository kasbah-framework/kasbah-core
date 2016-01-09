using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index
{
    public interface IIndexProvider
    {
        #region Public Methods

        void Delete(Guid id);

        IEnumerable<IDictionary<string, object>> Query(object query, int? limit = null);

        void Store(IDictionary<string, object> value);

        #endregion
    }
}