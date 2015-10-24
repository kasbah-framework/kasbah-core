namespace Kasbah.Core.Events
{
    public delegate void Message(string type, object data);

    public interface IEventService
    {
        event Message OnMessage;

        void Emit(string type, object data);

        void RegisterListener(string type, Message listener);
    }
}