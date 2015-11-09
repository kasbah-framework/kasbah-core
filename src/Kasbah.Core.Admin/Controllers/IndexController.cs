using System.Collections.Generic;
using Kasbah.Core.Index;
using Microsoft.AspNet.Mvc;

namespace Kasbah.Core.Admin
{
    public class IndexController
    {
        #region Public Constructors

        public IndexController(IndexService indexService)
        {
            _indexService = indexService;
        }

        #endregion

        #region Public Methods

        [HttpGet, Route("api/search")]
        public IEnumerable<IDictionary<string, object>> Search(string query)
        {
            return _indexService.Query(query);
        }

        #endregion

        #region Private Fields

        readonly IndexService _indexService;

        #endregion
    }
}
