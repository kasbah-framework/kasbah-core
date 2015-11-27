using System;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentTree.Events
{
    public class AfterItemSaved : ItemEventBase { }

    public class BeforeItemSaved : ItemEventBase { }

    public abstract class ItemEventBase : EventBase<object>
    {
        #region Public Properties

        public Guid Node { get; set; }

        public Guid Version { get; set; }

        #endregion
    }
}