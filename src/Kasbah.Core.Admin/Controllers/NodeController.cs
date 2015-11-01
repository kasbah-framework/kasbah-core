using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Models;
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
            return _contentTreeService.GetChildren(id).ToList();
        }
    }
}