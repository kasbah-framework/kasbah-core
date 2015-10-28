using Kasbah.Core.Events;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree.Events
{
    public abstract class ItemEventBase : EventBase<ItemBase> { }

    public class BeforeItemSaved : ItemEventBase { }

    public class AfterItemSaved : ItemEventBase { }
}