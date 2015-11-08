using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Events;
using Kasbah.Core.ContentTree;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Models;
using Kasbah.Core.Index.Utils;
using Kasbah.Core.Utils;

namespace Kasbah.Core.Index
{
    class ContentTreeEventHandler : IEventHandler
    {
        public ContentTreeEventHandler(IndexService indexService, IContentTreeService contentTreeService)
        {
            _indexService = indexService;
            _contentTreeService = contentTreeService;
        }

        public void HandleEvent<T>(T @event) where T : EventBase
        {
            var node = default(Node);
            var item = default(ItemBase);
            if (typeof(T) == typeof(AfterItemSaved))
            {
                node = _contentTreeService.GetNode((@event as AfterItemSaved).Node);
                item = (@event as AfterItemSaved).Data;
            }

            if (item != null && node != null)
            {
                var type = TypeUtil.TypeFromName(node.Type);
                var indexObject = _indexService.HandlePreIndex(item, type);

                var nodeFields = SerialisationUtil.Serialise(node);
                foreach (var field in nodeFields)
                {
                    indexObject[$"__node_{field.Key}"] = field.Value;
                }

            }
        }

        readonly IndexService _indexService;
        readonly IContentTreeService _contentTreeService;
    }
}