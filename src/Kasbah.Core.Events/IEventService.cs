using System;

namespace Kasbah.Core.Events
{
    public delegate void Message(Guid type, object data);

    public interface IEventService
    {
        event Message OnMessage;

        void Emit(Guid type, object data);

        void RegisterListener(Guid type, Message listener);
    }
}