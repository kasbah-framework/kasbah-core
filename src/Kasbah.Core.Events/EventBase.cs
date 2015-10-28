namespace Kasbah.Core.Events
{
    public abstract class EventBase
    {

    }

    public abstract class EventBase<T> : EventBase
    {
        public EventBase()
        {

        }

        public EventBase(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}