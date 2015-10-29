namespace Kasbah.Core.Events
{
    public interface IEventService
    {
        void Emit<T>(T @event) where T : EventBase;

        void Register<T>(IEventHandler handler) where T : EventBase;

        void Unregister(IEventHandler handler);

        void Unregister<T>(IEventHandler handler) where T : EventBase;
    }
}