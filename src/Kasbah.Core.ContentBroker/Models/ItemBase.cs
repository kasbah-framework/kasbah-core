using System;
using Kasbah.Core.Annotations;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentBroker.Models
{
    public sealed class EmptyItem : ItemBase
    {
    }

    public abstract class ItemBase
    {
        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        /// <value>
        /// The node identifier.
        /// </value>
        [SystemField]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the node associated with this content.
        /// </summary>
        /// <value>
        /// The node associated with this content.
        /// </value>
        [SystemField]
        public Node Node { get; internal set; }
    }
}