using System;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentBroker.Events
{
    public class NodeCreated : NodeEventBase { }

    public class NodeMoved : NodeEventBase { }

    public class NodeActiveVersionSet : NodeEventBase { }

    public abstract class NodeEventBase : EventBase<Guid> { }
}