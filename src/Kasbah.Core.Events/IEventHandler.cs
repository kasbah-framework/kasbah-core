namespace Kasbah.Core.Events
{
    public interface IEventHandler
    {
        void HandleEvent<T>(T @event) where T : EventBase;
    }
}