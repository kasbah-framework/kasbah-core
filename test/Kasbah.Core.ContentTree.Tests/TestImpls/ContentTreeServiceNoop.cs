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

        public override T GetActiveNodeVersion<T>(Guid id)
        {
            return default(T);
        }

        public override IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Node> GetChildren(Guid? id)
        {
            return Enumerable.Empty<Node>();
        }

        public override T GetMostRecentlyCreatedNodeVersion<T>(Guid id)
        {
            return default(T);
        }

        public override Node GetNode(Guid id)
        {
            throw new NotImplementedException();
        }

        public override T GetNodeVersion<T>(Guid id, Guid version)
        {
            throw new NotImplementedException();
        }

        public override object GetNodeVersion(Guid id, Guid version, Type type)
        {
            throw new NotImplementedException();
        }

        protected override Guid InternalCreateNode(Guid? parent, string alias)
        {
            return Guid.Empty;
        }

        protected override NodeVersion InternalSave<T>(Guid id, Guid nodeId, T item)
        {
            return null;
        }

        #endregion
    }
}