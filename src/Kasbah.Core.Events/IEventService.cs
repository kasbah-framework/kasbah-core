namespace Kasbah.Core.Events
{
    public class EventService
    {
        public EventService(IEventBusProvider eventBusProvider)
        {
            _eventBusProvider = eventBusProvider;
        }

        public void Emit<T>(T @event) where T : EventBase
            => _eventBusProvider.Emit<T>(@event);

        public void Register<T>(IEventHandler handler) where T : EventBase
            => _eventBusProvider.Register<T>(handler);

        public void Unregister(IEventHandler handler)
            => _eventBusProvider.Unregister(handler);

        public void Unregister<T>(IEventHandler handler) where T : EventBase
            => _eventBusProvider.Unregister<T>(handler);

        readonly IEventBusProvider _eventBusProvider;
    }
}