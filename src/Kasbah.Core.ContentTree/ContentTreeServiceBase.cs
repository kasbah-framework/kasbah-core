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
            var node = new Node { Id = Guid.Empty, Parent = parent, Alias = alias };

            _eventService.Emit(new BeforeNodeCreated { Data = node });

            node.Id = InternalCreateNode(parent, alias);

            _eventService.Emit(new AfterNodeCreated { Data = node });

            return node.Id;
        }

        public abstract IEnumerable<Node> GetChildren(Guid? id);

        public abstract IEnumerable<NodeVersion> GetAllNodeVersions(Guid id);

        public abstract T GetActiveNodeVersion<T>(Guid id) where T : ItemBase;

        public abstract T GetMostRecentlyCreatedNodeVersion<T>(Guid id) where T : ItemBase;

        public abstract Node GetNode(Guid id);

        public abstract T GetNodeVersion<T>(Guid id, Guid version) where T : ItemBase;

        public abstract object GetNodeVersion(Guid id, Guid version, Type type);

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

        protected abstract NodeVersion InternalSave<T>(Guid id, Guid nodeId, T item) where T : ItemBase;

        #endregion

        #region Private Fields

        readonly IEventService _eventService;

        #endregion
    }
}
