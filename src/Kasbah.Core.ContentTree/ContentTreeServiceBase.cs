using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Events;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public abstract class ContentTreeServiceBase : IContentTreeService
    {
        #region Public Constructors

        public ContentTreeServiceBase(IEventService eventService)
        {
            _eventService = eventService;
        }

        #endregion

        #region Public Methods

        public Guid CreateNode(Guid? parent, string alias)
        {
            var node = new Node { Id = Guid.Empty, ParentId = parent, Alias = alias };

            _eventService.Emit(new BeforeNodeCreated { Data = node });

            node.Id = InternalCreateNode(parent, alias);

            _eventService.Emit(new AfterNodeCreated { Data = node });

            return node.Id;
        }

        public IEnumerable<Tuple<T, DateTime>> GetAllItemVersions<T>(Guid id)
             where T : ItemBase
        {
            return InternalGetAllItemVersions<T>(id);
        }

        public abstract IEnumerable<Node> GetChildren(Guid? id);

        public T GetMostRecentlyCreatedItemVersion<T>(Guid id)
             where T : ItemBase
        {
            return InternalGetMostRecentlyCreatedItemVersion<T>(id);
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            throw new NotImplementedException();
        }

        public NodeVersion Save<T>(Guid id, Guid nodeId, T item)
            where T : ItemBase
        {
            _eventService.Emit(new BeforeItemSaved { Data = item });

            var ret = InternalSave<T>(id, nodeId, item);

            _eventService.Emit(new AfterItemSaved { Data = item });

            return ret;
        }

        #endregion

        #region Protected Methods

        protected abstract Guid InternalCreateNode(Guid? parent, string alias);

        protected abstract IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id) where T : ItemBase;

        protected abstract T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id) where T : ItemBase;

        protected abstract NodeVersion InternalSave<T>(Guid id, Guid nodeId, T item) where T : ItemBase;

        #endregion

        #region Private Fields

        readonly IEventService _eventService;

        #endregion
    }
}