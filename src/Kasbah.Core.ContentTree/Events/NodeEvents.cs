using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentTree.Events
{
    public class AfterNodeCreated : NodeEventBase { }

    public class AfterNodeMoved : EventBase<Node> { }

    public class BeforeNodeCreated : NodeEventBase { }

    public class BeforeNodeMoved : EventBase<Node> { }

    public abstract class NodeEventBase : EventBase<Node> { }
}