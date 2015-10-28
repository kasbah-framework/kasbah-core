using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentTree.Events
{
    public abstract class NodeEventBase : EventBase<Node> { }

    public class BeforeNodeCreated : NodeEventBase { }

    public class AfterNodeCreated : NodeEventBase { }
}