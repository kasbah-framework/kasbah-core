using System;
using Kasbah.Core.Annotations;

namespace Kasbah.Core.Models
{
    public sealed class EmptyItem : ItemBase
    {
    }

    public abstract class ItemBase : MarshalByRefObject
    {
        [SystemField]
        public Guid Id { get; set; }

        [SystemField]
        public Node Node { get; set; }
    }
}