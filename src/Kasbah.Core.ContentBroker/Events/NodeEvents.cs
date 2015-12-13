using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentBroker.Events
{
    public class AfterNodeCreated : NodeEventBase { }

    public class AfterNodeMoved : NodeEventBase { }

    public class BeforeNodeCreated : NodeEventBase { }

    public class BeforeNodeMoved : NodeEventBase { }

    public class NodeActiveVersionSet : NodeEventBase { }

    public abstract class NodeEventBase : EventBase<Node> { }
}