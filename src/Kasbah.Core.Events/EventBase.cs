namespace Kasbah.Core.Events
{
    public abstract class EventBase
    {
    }

    public abstract class EventBase<T> : EventBase
    {
        #region Public Constructors

        public EventBase()
        {
        }

        public EventBase(T data)
        {
            Data = data;
        }

        #endregion

        #region Public Properties

        public T Data { get; set; }

        #endregion
    }
}