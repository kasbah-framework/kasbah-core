using System;
using Kasbah.Core.Models;
using System.Collections.Generic;
using Kasbah.Core.Events;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Models;

namespace Kasbah.Core.ContentTree
{
    public abstract class ContentTreeServiceBase : IContentTreeService
    {
        readonly EventService _eventService;
        public ContentTreeServiceBase(EventService eventService)
        {
            _eventService = eventService;
        }

        protected abstract IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id) where T : ItemBase;
        protected abstract T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id) where T : ItemBase;
        protected abstract void InternalSave<T>(Guid id, Guid nodeId, T item) where T : ItemBase;

        protected abstract void InternalCreateNode(Guid id, Guid? parent, string alias);


        public IEnumerable<Tuple<T, DateTime>> GetAllItemVersions<T>(Guid id)
             where T : ItemBase
        {
            return InternalGetAllItemVersions<T>(id);
        }


        public T GetMostRecentlyCreatedItemVersion<T>(Guid id)
             where T : ItemBase
        {
            return InternalGetMostRecentlyCreatedItemVersion<T>(id);
        }


        public void Save<T>(Guid id, Guid nodeId, T item)
            where T : ItemBase
        {
            _eventService.Emit(new BeforeItemSaved { Data = item });

            InternalSave<T>(id, nodeId, item);

            _eventService.Emit(new AfterItemSaved { Data = item });
        }

        public void CreateNode(Guid id, Guid? parent, string alias)
        {
            var node = new Node { Id = id, ParentId = parent };

            _eventService.Emit(new BeforeNodeCreated { Data = node });

            InternalCreateNode(id, parent, alias);

            _eventService.Emit(new AfterNodeCreated { Data = node });
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            throw new NotImplementedException();
        }
    }
}