// using System;
// using Kasbah.Core.ContentBroker.Events;
// using Kasbah.Core.ContentTree;
// using Kasbah.Core.ContentTree.Models;
// using Kasbah.Core.Events;
// using Kasbah.Core.Index;
// using Kasbah.Core.Index.Utils;
// using Kasbah.Core.Utils;

// namespace Kasbah.Core.ContentBroker
// {
//     class ContentTreeEventHandler : IEventHandler
//     {
//         #region Public Constructors

//         public ContentTreeEventHandler(IndexService indexService, ContentTreeService contentTreeService)
//         {
//             _indexService = indexService;
//             _contentTreeService = contentTreeService;
//         }

//         #endregion

//         #region Public Methods

//         public void HandleEvent<T>(T @event) where T : EventBase
//         {
//             var node = default(Node);
//             var item = default(object);
//             var version = default(Guid?);
//             if (typeof(T) == typeof(AfterItemSaved))
//             {
//                 var _event = (@event as AfterItemSaved);
//                 node = _contentTreeService.GetNode(_event.Node);
//                 item = _event.Payload;
//                 version = _event.Version;
//             }
//             else if (typeof(T) == typeof(NodeActiveVersionSet))
//             {
//                 var _event = (@event as NodeActiveVersionSet);
//                 node = _event.Payload;
//                 version = _event.Payload.ActiveVersion;
//                 if (version.HasValue)
//                 {
//                     item = _contentTreeService.GetNodeVersion(node.Id, version.Value);
//                 }

//                 // TODO: set other to active = false
//             }

//             if (item != null && node != null)
//             {
//                 var type = TypeUtil.TypeFromName(node.Type);
//                 var indexObject = _indexService.HandlePreIndex(item, type);

//                 indexObject["id"] = version.Value;

//                 indexObject["_active"] = node.ActiveVersion == version;

//                 var nodeFields = SerialisationUtil.Serialise(node);
//                 foreach (var field in nodeFields)
//                 {
//                     indexObject[$"__node_{field.Key}"] = field.Value;
//                 }

//                 _indexService.Store(indexObject);
//             }
//         }

//         #endregion

//         #region Private Fields

//         readonly ContentTreeService _contentTreeService;
//         readonly IndexService _indexService;

//         #endregion
//     }
// }