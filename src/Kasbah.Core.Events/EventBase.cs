namespace Kasbah.Core.Events
{
    public abstract class EventBase
    {
    }

    public abstract class EventBase<T> : EventBase
    {
        #region Public Properties

        public T Payload { get; set; }

        #endregion
    }
}