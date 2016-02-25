using System;
using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public interface IContentTreeProvider
    {
        #region Public Methods

        /// <summary>
        /// Creates a new node under <paramref name="parent"/> with <paramref name="alias"/> of
        /// <paramref name="type"/> with node identifier <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <param name="parent">The parent node identifier.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="type">The type.</param>
        void CreateNode(Guid id, Guid? parent, string alias, string type);

        /// <summary>
        /// Deletes the node with specified identifier.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        void Delete(Guid id);

        /// <summary>
        /// Gets all node versions.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <returns>All node versions related to node with identifier <paramref name="id"/>.</returns>
        IEnumerable<NodeVersion> GetAllNodeVersions(Guid id);

        /// <summary>
        /// Gets the child node under <paramref name="parent"/> with <paramref name="alias"/>.
        /// </summary>
        /// <param name="parent">The parent node identifier.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The requested child node if it exists, otherwise null.</returns>
        Node GetChild(Guid? parent, string alias);

        /// <summary>
        /// Gets the children nodes under the node with identifier <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <returns>All children nodes under the specified node.</returns>
        IEnumerable<Node> GetChildren(Guid? id);

        /// <summary>
        /// Gets a single node with identifier.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <returns>The requested node if it exists, otherwise null.</returns>
        Node GetNode(Guid id);

        /// <summary>
        /// Gets the specified node version.
        /// </summary>
        /// <param name="node">The node identifier.</param>
        /// <param name="version">The version identifier.</param>
        /// <returns>The requested node version.</returns>
        IDictionary<string, object> GetNodeVersion(Guid id, Guid version);

        /// <summary>
        /// Gets the raw node version.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <param name="version">The version identifier.</param>
        /// <returns>The raw representation of the node version - not strongly typed.</returns>
        NodeVersion GetRawNodeVersion(Guid id, Guid version);

        /// <summary>
        /// Moves the specified node to a new location.
        /// </summary>
        /// <param name="id">The node identifier.</param>
        /// <param name="parent">The new parent node identifier.</param>
        void MoveNode(Guid id, Guid? parent);

        /// <summary>
        /// Saves the node version.
        /// </summary>
        /// <param name="node">The node identifier.</param>
        /// <param name="version">The version identifier.</param>
        /// <param name="item">The item data.</param>
        /// <returns>
        /// The node version that has been saved.  If the version identifier doesn't
        /// exist it will be created, otherwise it will be updated.
        /// </returns>
        NodeVersion Save(Guid id, Guid node, IDictionary<string, object> item);

        /// <summary>
        /// Sets the active node version.
        /// </summary>
        /// <param name="node">The node identifier.</param>
        /// <param name="version">The version identifier.</param>
        void SetActiveNodeVersion(Guid id, Guid? version);

        #endregion
    }
}