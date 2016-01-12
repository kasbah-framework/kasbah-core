using System;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentBroker.Events
{
    public class NodeActiveVersionSet : NodeEventBase { }

    public class NodeCreated : NodeEventBase { }

    public abstract class NodeEventBase : EventBase<Guid> { }

    public class NodeMoved : NodeEventBase { }
}