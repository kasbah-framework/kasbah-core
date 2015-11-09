using System;
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
        public ContentTreeEventHandler(IndexService indexService, IIndexProvider indexProvider, ContentTreeService contentTreeService)
        {
            _indexService = indexService;
            _indexProvider = indexProvider;
            _contentTreeService = contentTreeService;
        }

        public void HandleEvent<T>(T @event) where T : EventBase
        {
            var node = default(Node);
            var item = default(object);
            var version = default(Guid?);
            if (typeof(T) == typeof(AfterItemSaved))
            {
                var _event = (@event as AfterItemSaved);
                node = _contentTreeService.GetNode(_event.Node);
                item = _event.Data;
                version = _event.Version;
            }
            else if (typeof(T) == typeof(NodeActiveVersionSet))
            {
                var _event = (@event as NodeActiveVersionSet);
                node = _event.Data;
                version = _event.Data.ActiveVersionId;
                if (version.HasValue)
                {
                    item = _contentTreeService.GetNodeVersion(node.Id, version.Value);
                }

                // TODO: set other to active = false
            }

            if (item != null && node != null)
            {
                var type = TypeUtil.TypeFromName(node.Type);
                var indexObject = _indexService.HandlePreIndex(item, type);

                indexObject["id"] = version.Value;

                indexObject["_active"] = node.ActiveVersionId == version;

                var nodeFields = SerialisationUtil.Serialise(node);
                foreach (var field in nodeFields)
                {
                    indexObject[$"__node_{field.Key}"] = field.Value;
                }

                _indexProvider.Store(indexObject);
            }
        }

        readonly IndexService _indexService;
        readonly ContentTreeService _contentTreeService;
        readonly IIndexProvider _indexProvider;
    }
}