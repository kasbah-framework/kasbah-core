using System;
using Kasbah.Core.Models;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentBroker.Events
{
    public class ItemSaved : ItemEventBase { }

    public abstract class ItemEventBase : EventBase<ItemBase>
    {
        #region Public Properties

        public Guid Node { get; set; }

        public Guid Version { get; set; }

        #endregion
    }
}