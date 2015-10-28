using System;
using Kasbah.Core.Models;
using System.Collections.Generic;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentTree
{
    public abstract class ContentTreeServiceBase : IContentTreeService
    {
        readonly IEventService _eventService;
        public ContentTreeServiceBase(IEventService eventService)
        {
            _eventService = eventService;
        }

        protected abstract IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id) where T : ItemBase;
        protected abstract T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id) where T : ItemBase;
        protected abstract void InternalSave<T>(Guid id, T item) where T : ItemBase;


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


        public void Save<T>(Guid id, T item)
            where T : ItemBase
        {
            _eventService.Emit("before:item_save", new { Id = id });

            InternalSave<T>(id, item);

            _eventService.Emit("after:item_save", new { Id = id });
        }
    }
}