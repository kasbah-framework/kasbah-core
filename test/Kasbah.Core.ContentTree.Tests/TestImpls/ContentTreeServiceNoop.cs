using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentTree.Tests.TestImpls
{
    internal class ContentTreeServiceNoop : ContentTreeServiceBase
    {
        #region Public Constructors

        public ContentTreeServiceNoop(IEventService eventService) : base(eventService)
        {
        }

        #endregion

        #region Protected Methods

        public override IEnumerable<Node> GetChildren(Guid? id)
        {
            return Enumerable.Empty<Node>();
        }

        protected override Guid InternalCreateNode(Guid? parent, string alias)
        {
            return Guid.Empty;
        }

        public override IEnumerable<Tuple<NodeVersion, T>> GetAllNodeVersions<T>(Guid id)
        {
            return Enumerable.Empty<Tuple<NodeVersion, T>>();
        }

        public override T GetMostRecentlyCreatedNodeVersion<T>(Guid id)
        {
            return default(T);
        }

        public override T GetActiveNodeVersion<T>(Guid id)
        {
            return default(T);
        }

        protected override NodeVersion InternalSave<T>(Guid id, Guid nodeId, T item)
        {
            return null;
        }

        #endregion
    }
}
