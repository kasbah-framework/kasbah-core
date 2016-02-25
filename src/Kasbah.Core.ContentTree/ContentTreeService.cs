using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public class ContentTreeService
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTreeService"/> class.
        /// </summary>
        /// <param name="contentTreeProvider">The content tree provider.</param>
        public ContentTreeService(IContentTreeProvider contentTreeProvider)
        {
            _contentTreeProvider = contentTreeProvider;
        }

        #endregion

        #region Public Methods

        /// <see cref="CreateNode(Guid?, string, string)"/>
        public Guid CreateNode(Guid? parent, string alias, Type type)
            => CreateNode(parent, alias, type.AssemblyQualifiedName);

        /// <see cref="IContentTreeProvider.CreateNode(Guid, Guid?, string, string)"/>
        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var id = Guid.NewGuid();

            _contentTreeProvider.CreateNode(id, parent, alias, type);

            return id;
        }

        /// <see cref="IContentTreeProvider.Delete(Guid)"/>
        public void Delete(Guid id)
            => _contentTreeProvider.Delete(id);

        /// <see cref="IContentTreeProvider.GetAllNodeVersions(Guid)"/>
        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
            => _contentTreeProvider.GetAllNodeVersions(id);

        /// <see cref="IContentTreeProvider.GetChild(Guid?, string)"/>
        public Node GetChild(Guid? parent, string alias)
            => _contentTreeProvider.GetChild(parent, alias);

        /// <see cref="IContentTreeProvider.GetChildren(Guid?)"/>
        public IEnumerable<Node> GetChildren(Guid? id)
            => _contentTreeProvider.GetChildren(id);

        /// <see cref="IContentTreeProvider.GetNode(Guid)"/>
        public Node GetNode(Guid id)
            => _contentTreeProvider.GetNode(id);

        /// <see cref="IContentTreeProvider.GetNodeVersion(Guid, Guid)"/>
        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
            => _contentTreeProvider.GetNodeVersion(id, version);

        /// <see cref="GetOrCreate(Guid?, string, Type)"/>
        public Guid GetOrCreate<T>(Guid? parent, string alias)
            => GetOrCreate(parent, alias, typeof(T));

        /// <summary>
        /// Gets a node with <paramref name="alias"/> under <paramref name="parent"/> if
        /// it exists, otherwise creates it.
        /// </summary>
        /// <param name="parent">The parent node identifier.</param>
        /// <param name="alias">The node alias.</param>
        /// <param name="type">The type.</param>
        /// <returns>The node identifier.</returns>
        public Guid GetOrCreate(Guid? parent, string alias, Type type)
        {
            var ret = GetChild(parent, alias);
            if (ret == null)
            {
                return CreateNode(parent, alias, type);
            }

            return ret.Id;
        }

        /// <see cref="IContentTreeProvider.GetRawNodeVersion(Guid, Guid)"/>
        public NodeVersion GetRawNodeVersion(Guid id, Guid version)
            => _contentTreeProvider.GetRawNodeVersion(id, version);

        /// <see cref="IContentTreeProvider.MoveNode(Guid, Guid?)"/>
        public void MoveNode(Guid id, Guid? parent)
            => _contentTreeProvider.MoveNode(id, parent);

        /// <see cref="IContentTreeProvider.Save(Guid, Guid, IDictionary{string, object})"/>
        public NodeVersion Save(Guid id, Guid node, IDictionary<string, object> item)
            => _contentTreeProvider.Save(id, node, item);

        /// <see cref="IContentTreeProvider.SetActiveNodeVersion(Guid, Guid?)"/>
        public void SetActiveNodeVersion(Guid id, Guid? version)
            => _contentTreeProvider.SetActiveNodeVersion(id, version);

        #endregion

        #region Private Fields

        readonly IContentTreeProvider _contentTreeProvider;

        #endregion
    }
}