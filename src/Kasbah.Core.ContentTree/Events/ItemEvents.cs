using Kasbah.Core.Events;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree.Events
{
    public class AfterItemSaved : ItemEventBase { }

    public class BeforeItemSaved : ItemEventBase { }

    public abstract class ItemEventBase : EventBase<ItemBase> { }
}