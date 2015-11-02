using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Models;
using Microsoft.AspNet.Mvc;

namespace Kasbah.Core.Admin
{
    public class NodeController
    {
        #region Public Constructors

        public NodeController(IContentTreeService contentTreeService)
        {
            _contentTreeService = contentTreeService;
        }

        #endregion

        #region Public Methods

        [Route("api/children")]
        public IEnumerable<Node> GetChildren(Guid? id = null)
        {
            return _contentTreeService.GetChildren(id);
        }

        [Route("api/version/{id}/{version}")]
        public IDictionary<string, object> GetVersion(Guid id, Guid version)
        {
            return _contentTreeService.GetNodeVersion(id, version);
        }

        [Route("api/versions/{id}")]
        public IEnumerable<NodeVersion> GetVersions(Guid id)
        {
            return _contentTreeService.GetAllNodeVersions(id);
        }

        #endregion

        #region Private Fields

        readonly IContentTreeService _contentTreeService;

        #endregion
    }
}