using Kasbah.Core.Events;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentBroker.Events
{
    public class AfterItemIndexed : ItemEventBase { }

    public class BeforeItemIndexed : ItemEventBase { }

    public abstract class ItemEventBase : EventBase<ItemBase> { }
}