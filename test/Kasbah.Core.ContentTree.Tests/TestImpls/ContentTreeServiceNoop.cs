using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree.Tests.TestImpls
{
    internal class ContentTreeServiceNoop : IContentTreeService
    {
        #region Public Methods

        public Guid CreateNode<T>(Guid? parent, string alias) where T : ItemBase
        {
            throw new NotImplementedException();
        }

        public T GetActiveNodeVersion<T>(Guid id) where T : ItemBase
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            throw new NotImplementedException();
        }

        public Node GetChild(Guid? parent, string alias)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Node GetNode(Guid id)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            throw new NotImplementedException();
        }

        public object GetNodeVersion(Guid id, Guid version, Type type)
        {
            throw new NotImplementedException();
        }

        public T GetNodeVersion<T>(Guid id, Guid version) where T : ItemBase
        {
            throw new NotImplementedException();
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            throw new NotImplementedException();
        }

        public NodeVersion Save<T>(Guid id, Guid nodeId, T item) where T : ItemBase
        {
            throw new NotImplementedException();
        }

        public void SetActiveNodeVersion(Guid id, Guid version)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}