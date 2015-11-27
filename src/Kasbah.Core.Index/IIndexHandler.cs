using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index
{
    public interface IIndexHandler
    {
        #region Public Properties

        int Priority { get; }

        #endregion

        #region Public Methods

        IDictionary<string, object> AddCustomFields(ItemBase item);

        #endregion
    }
}