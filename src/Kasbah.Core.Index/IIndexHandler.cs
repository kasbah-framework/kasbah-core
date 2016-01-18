using System.Collections.Generic;

namespace Kasbah.Core.Index
{
    public interface IIndexHandler
    {
        #region Public Properties

        int Priority { get; }

        #endregion

        #region Public Methods

        IDictionary<string, object> AddCustomFields(IDictionary<string, object> item);

        #endregion
    }
}