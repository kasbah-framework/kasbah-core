using System.Collections.Generic;
using Kasbah.Core.Events;

namespace Kasbah.Core.ContentTree.Tests.TestImpls
{
    internal class BasicEventHandler : IEventHandler
    {
        #region Public Fields

        public ICollection<EventBase> HandledEvents = new List<EventBase>();

        #endregion

        #region Public Methods

        public void HandleEvent<T>(T @event) where T : EventBase
        {
            HandledEvents.Add(@event);
        }

        #endregion
    }
}