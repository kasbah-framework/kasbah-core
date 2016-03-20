using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.Tree
{
    public class TreeService
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeService"/> class.
        /// </summary>
        /// <param name="treeProvider">The content tree provider.</param>
        public TreeService(ITreeProvider treeProvider)
        {
            _treeProvider = treeProvider;
        }

        #endregion

        #region Public Methods

        /// <see cref="CreateNode(Guid?, string, string)"/>
        public Guid CreateNode(Guid? parent, string alias, Type type)
            => CreateNode(parent, alias, type.AssemblyQualifiedName);

        /// <see cref="ITreeProvider.CreateNode(Guid, Guid?, string, string)"/>
        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var id = Guid.NewGuid();

            _treeProvider.CreateNode(id, parent, alias, type);

            return id;
        }

        /// <see cref="ITreeProvider.Delete(Guid)"/>
        public void Delete(Guid id)
            => _treeProvider.Delete(id);

        /// <see cref="ITreeProvider.GetAllNodeVersions(Guid)"/>
        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
            => _treeProvider.GetAllNodeVersions(id);

        /// <see cref="ITreeProvider.GetChild(Guid?, string)"/>
        public Node GetChild(Guid? parent, string alias)
            => _treeProvider.GetChild(parent, alias);

        /// <see cref="ITreeProvider.GetChildren(Guid?)"/>
        public IEnumerable<Node> GetChildren(Guid? id)
            => _treeProvider.GetChildren(id);

        /// <see cref="ITreeProvider.GetNode(Guid)"/>
        public Node GetNode(Guid id)
            => _treeProvider.GetNode(id);

        /// <see cref="ITreeProvider.GetNodeVersion(Guid, Guid)"/>
        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
            => _treeProvider.GetNodeVersion(id, version);

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

        /// <see cref="ITreeProvider.GetRawNodeVersion(Guid, Guid)"/>
        public NodeVersion GetRawNodeVersion(Guid id, Guid version)
            => _treeProvider.GetRawNodeVersion(id, version);

        /// <see cref="ITreeProvider.MoveNode(Guid, Guid?)"/>
        public void MoveNode(Guid id, Guid? parent)
            => _treeProvider.MoveNode(id, parent);

        /// <see cref="ITreeProvider.Save(Guid, Guid, IDictionary{string, object})"/>
        public NodeVersion Save(Guid id, Guid node, IDictionary<string, object> item)
            => _treeProvider.Save(id, node, item);

        /// <see cref="ITreeProvider.SetActiveNodeVersion(Guid, Guid?)"/>
        public void SetActiveNodeVersion(Guid id, Guid? version)
            => _treeProvider.SetActiveNodeVersion(id, version);

        #endregion

        #region Private Fields

        readonly ITreeProvider _treeProvider;

        #endregion
    }
}