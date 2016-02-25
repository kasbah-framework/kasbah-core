namespace Kasbah.Core.Events
{
    public interface IEventBusProvider
    {
        #region Public Methods

        /// <summary>
        /// Emits the specified event.
        /// </summary>
        /// <typeparam name="T">The type of event to emit.</typeparam>
        /// <param name="event">The event.</param>
        void Emit<T>(T @event) where T : EventBase;

        /// <summary>
        /// Registers the for events of a specific type.
        /// </summary>
        /// <typeparam name="T">The type of event to listen for.</typeparam>
        /// <param name="handler">The handler.</param>
        void Register<T>(IEventHandler handler) where T : EventBase;

        /// <summary>
        /// Unregisters the specified handler from all events.
        /// </summary>
        /// <param name="handler">The handler.</param>
        void Unregister(IEventHandler handler);

        /// <summary>
        /// Unregisters the specified handler from listening to events of a specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        void Unregister<T>(IEventHandler handler) where T : EventBase;

        #endregion
    }
}