using Kasbah.Core.ContentTree;
using Kasbah.Core.Index;
using Kasbah.Core.Events;
using Kasbah.Core.ContentTree.Models;
using System.Collections.Generic;

namespace Kasbah.Core.ContentBroker
{
    public partial class ContentBroker
    {
        public ContentBroker(ContentTreeService contentTreeService, IndexService indexService, EventService eventService)
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
        readonly EventService _eventService;
    }
}