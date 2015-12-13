using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentBroker.Events
{
    public class AfterItemSaved : ItemEventBase { }

    public class BeforeItemSaved : ItemEventBase { }

    public abstract class ItemEventBase : EventBase<ItemBase>
    {
        #region Public Properties

        public Guid Node { get; set; }

        public Guid Version { get; set; }

        #endregion
    }
}