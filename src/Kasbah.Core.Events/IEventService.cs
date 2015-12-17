namespace Kasbah.Core.Events
{
    public class EventService
    {
        #region Public Constructors

        public EventService(IEventBusProvider eventBusProvider)
        {
            _eventBusProvider = eventBusProvider;
        }

        #endregion

        #region Public Methods

        public void Emit<T>(T @event) where T : EventBase
            => _eventBusProvider.Emit<T>(@event);

        public void Register<T>(IEventHandler handler) where T : EventBase
            => _eventBusProvider.Register<T>(handler);

        public void Unregister(IEventHandler handler)
            => _eventBusProvider.Unregister(handler);

        public void Unregister<T>(IEventHandler handler) where T : EventBase
            => _eventBusProvider.Unregister<T>(handler);

        #endregion

        #region Private Fields

        readonly IEventBusProvider _eventBusProvider;

        #endregion
    }
}