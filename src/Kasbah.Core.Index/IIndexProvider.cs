using System;
using System.Collections.Generic;

namespace Kasbah.Core.Index
{
    public interface IIndexProvider
    {
        #region Public Methods

        /// <summary>
        /// Deletes the item with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(Guid id);

        /// <summary>
        /// Queries the node index.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="type">The type of content to return.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to take.</param>
        /// <param name="sort">The sort order.</param>
        /// <returns>A list of objects that match the query.</returns>
        IEnumerable<IDictionary<string, object>> Query(object query, int? skip = null, int? take = null, string sort = null);

        /// <summary>
        /// Stores the specified value in the index.
        /// </summary>
        /// <param name="value">The value to store.</param>
        /// <param name="type">The type of value being stored.</param>
        void Store(IDictionary<string, object> value);

        #endregion
    }
}