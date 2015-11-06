using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Models;
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

        [HttpPost, Route("api/node/{id}/version")]
        public NodeVersion CreateNodeVersion(Guid id)
        {
            return _contentTreeService.Save<ItemBase>(Guid.NewGuid(), id, null);
        }


        [HttpPost, Route("api/node")]
        public Guid CreateNode([FromBody]CreateNodeRequest request)
        {
            return _contentTreeService.CreateNode(request.Parent, request.Alias, request.Type);
        }

        #endregion

        #region Private Fields

        readonly IContentTreeService _contentTreeService;

        #endregion
    }

    public class CreateNodeRequest
    {
        public Guid? Parent { get; set; }

        public string Alias { get; set; }

        public string Type { get; set; }
    }
}