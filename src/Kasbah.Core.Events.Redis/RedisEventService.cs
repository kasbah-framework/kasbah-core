using System;

namespace Kasbah.Core.Events.Redis
{
    public class RedisEventService : IEventService
    {
        public RedisEventService()
        {

        }

        public void Emit<T>(T @event) where T : EventBase
        {
            throw new NotImplementedException();
        }

        public void Register<T>(IEventHandler handler) where T : EventBase
        {
            throw new NotImplementedException();
        }

        public void Unregister(IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Unregister<T>(IEventHandler handler) where T : EventBase
        {
            throw new NotImplementedException();
        }

        object GetConnection()
        {
            return null;
        }
    }
}