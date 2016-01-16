using System;
using Kasbah.Core.Annotations;

namespace Kasbah.Core.Models
{
    public sealed class EmptyItem : ItemBase
    {
    }

#if DNXCORE50
    public abstract class ItemBase
#else

    public abstract class ItemBase : MarshalByRefObject
#endif
    {
        [SystemField]
        public Guid Id { get; set; }

        [SystemField]
        public Node Node { get; }
    }
}