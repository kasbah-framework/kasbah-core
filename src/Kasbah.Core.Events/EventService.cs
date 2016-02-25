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

        /// <see cref="IEventBusProvider.Emit{T}(T)"/>
        public void Emit<T>(T @event) where T : EventBase
            => _eventBusProvider.Emit<T>(@event);

        /// <see cref="IEventBusProvider.Register{T}(IEventHandler)"/>
        public void Register<T>(IEventHandler handler) where T : EventBase
            => _eventBusProvider.Register<T>(handler);

        /// <see cref="IEventBusProvider.Unregister(IEventHandler)"/>
        public void Unregister(IEventHandler handler)
            => _eventBusProvider.Unregister(handler);

        /// <see cref="IEventBusProvider.Unregister{T}(IEventHandler)"/>
        public void Unregister<T>(IEventHandler handler) where T : EventBase
            => _eventBusProvider.Unregister<T>(handler);

        #endregion

        #region Private Fields

        readonly IEventBusProvider _eventBusProvider;

        #endregion
    }
}