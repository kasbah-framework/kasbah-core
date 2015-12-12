using Kasbah.Core.ContentTree;
using Kasbah.Core.Index;
using Kasbah.Core.Events;
using Kasbah.Core.ContentTree.Models;
using System.Collections.Generic;

namespace Kasbah.Core
{
    public partial class ContentBroker
    {
        public ContentBroker(ContentTreeService contentTreeService, IndexService indexService, IEventService eventService)
        {
            _contentTreeService = contentTreeService;
            _indexService = indexService;
            _eventService = eventService;
        }

        public Node GetNodeByPath(IEnumerable<string> path)
        {
            var ret = default(Node);

            foreach (var item in path)
            {
                ret = GetChild(ret?.Id, item);
            }

            return ret;
        }

        readonly ContentTreeService _contentTreeService;
        readonly IndexService _indexService;
        readonly IEventService _eventService;
    }
}