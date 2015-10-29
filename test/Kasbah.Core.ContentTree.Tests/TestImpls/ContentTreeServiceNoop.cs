using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentTree.Tests.TestImpls
{
    internal class ContentTreeServiceNoop : ContentTreeServiceBase
    {
        #region Public Constructors

        public ContentTreeServiceNoop(EventService eventService) : base(eventService)
        {
        }

        #endregion

        #region Protected Methods

        protected override Guid InternalCreateNode(Guid? parent, string alias)
        {
            return Guid.Empty;
        }

        protected override IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id)
        {
            return Enumerable.Empty<Tuple<T, DateTime>>();
        }

        protected override T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id)
        {
            return default(T);
        }

        protected override void InternalSave<T>(Guid id, Guid nodeId, T item)
        {
        }

        #endregion
    }
}