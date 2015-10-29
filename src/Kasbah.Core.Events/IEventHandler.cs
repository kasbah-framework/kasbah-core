namespace Kasbah.Core.Events
{
    public interface IEventHandler
    {
        #region Public Methods

        void HandleEvent<T>(T @event) where T : EventBase;

        #endregion
    }
}