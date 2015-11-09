using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Index;
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
            return _contentTreeService.GetNodeVersion(id, version) ?? new Dictionary<string, object>();
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

        [HttpPost, Route("api/node/{id}/set-active/{version}")]
        public void SetActiveVersion(Guid id, Guid version)
        {
            _contentTreeService.SetActiveNodeVersion(id, version);
        }

        [HttpPost, Route("api/save/{node}/{version}")]
        public void Save(Guid node, Guid version, [FromBody]IDictionary<string, object> values)
        {
            _contentTreeService.Save(version, node, (object)values);
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
