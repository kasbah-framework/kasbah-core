using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public class ContentTreeService
    {
        #region Public Constructors

        public ContentTreeService(IContentTreeProvider contentTreeProvider)
        {
            _contentTreeProvider = contentTreeProvider;
        }

        #endregion

        #region Public Methods
        
        public Guid CreateNode(Guid? parent, string alias, Type type)
            => CreateNode(parent, alias, type.AssemblyQualifiedName);

        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var id = Guid.NewGuid();

            var node = new Node { Id = id, Parent = parent, Alias = alias, Type = type };

            _contentTreeProvider.CreateNode(id, parent, alias, type);

            return id;
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
            => _contentTreeProvider.GetAllNodeVersions(id);

        public Node GetChild(Guid? parent, string alias)
            => _contentTreeProvider.GetChild(parent, alias);

        public IEnumerable<Node> GetChildren(Guid? id)
            => _contentTreeProvider.GetChildren(id);

        public Node GetNode(Guid id)
            => _contentTreeProvider.GetNode(id);

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
            => _contentTreeProvider.GetNodeVersion(id, version);

        public Guid GetOrCreate(Guid? parent, string alias, Type type)
        {
            var ret = GetChild(parent, alias);
            if (ret == null)
            {
                return CreateNode(parent, alias, type);
            }

            return ret.Id;
        }
        

        public NodeVersion GetRawNodeVersion(Guid id, Guid version)
            => _contentTreeProvider.GetRawNodeVersion(id, version);

        public void MoveNode(Guid id, Guid? parent)
        {
            var node = new Node { Id = id, Parent = parent };

            _contentTreeProvider.MoveNode(id, parent);
        }

        public NodeVersion Save(Guid id, Guid node, IDictionary<string, object> item)
            => _contentTreeProvider.Save(id, node, item);

        public void SetActiveNodeVersion(Guid id, Guid? version)
            => _contentTreeProvider.SetActiveNodeVersion(id, version);

        #endregion

        #region Private Fields

        readonly IContentTreeProvider _contentTreeProvider;

        #endregion
    }
}