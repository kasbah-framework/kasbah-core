using System;
using Kasbah.Core.Annotations;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentBroker.Models
{
    public sealed class EmptyItem : ItemBase
    {
    }

    public abstract class ItemBase
#if !DNXCORE50
        : MarshalByRefObject
#endif
    {
        [SystemField]
        public Guid Id { get; set; }

        [SystemField]
        public Node Node { get; internal set; }
    }
}