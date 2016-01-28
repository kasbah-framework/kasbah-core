using System;
using System.Collections.Generic;
using Kasbah.Core.ContentBroker.Models;

namespace Kasbah.Core.ContentBroker.Extensions
{
    public static class ItemBaseExtensions
    {
        #region Public Methods

        /// <summary>
        /// Returns a list of Node identifiers the current item references
        /// </summary>
        /// <returns>A list of Node identifiers the current item references</returns>
        public static IEnumerable<Guid> GetNodeDependencies(this ItemBase item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}