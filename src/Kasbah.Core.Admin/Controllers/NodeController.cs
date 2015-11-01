using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Models;
using Microsoft.AspNet.Mvc;

namespace Kasbah.Core.Admin
{
    public class NodeController
    {
        readonly IContentTreeService _contentTreeService;

        public NodeController(IContentTreeService contentTreeService)
        {
            _contentTreeService = contentTreeService;
        }

        [Route("api/children")]
        public IEnumerable<Node> GetChildren(Guid? id = null)
        {
            return _contentTreeService.GetChildren(id);
        }

        [Route("api/versions/{id}")]
        public IEnumerable<NodeVersion> GetVersions(Guid id)
        {
            return _contentTreeService.GetAllNodeVersions(id);
        }

        [Route("api/version/{id}/{version}")]
        public object GetVersion(Guid id, Guid version)
        {
            var node = _contentTreeService.GetNode(id);
            var type = Type.GetType(node.Type);
            return _contentTreeService.GetNodeVersion(id, version, type);
        }
    }
}