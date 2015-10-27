namespace Kasbah.Core.Events
{
    public delegate void KasbahEventHandler(string type, object data);

    public interface IEventService
    {
        void Emit(string type);
        void Emit(string type, object data);

        void RegisterListener(string type, KasbahEventHandler handler);
    }
}