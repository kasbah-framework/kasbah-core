using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.Events;
using Xunit;

namespace Kasbah.Core.ContentTree.Tests
{
    public class NodeOperations
    {
        #region Public Methods

        [Fact]
        public void CreateNode_TriggersBeforeAndAfterCreateEvents_EventsTriggered()
        {
            // Arrange
            var eventService = new EventService();
            var handler = new EventHandler();

            eventService.Register<BeforeNodeCreated>(handler);
            eventService.Register<AfterNodeCreated>(handler);

            var service = new ContentTreeServiceImpl(eventService);

            // Act
            service.CreateNode(null, "test");

            // Assert
            Assert.NotEmpty(handler.HandledEvents);

            Assert.Equal(2, handler.HandledEvents.Count);
        }

        #endregion

        #region Internal Classes

        internal class ContentTreeServiceImpl : ContentTreeServiceBase
        {
            #region Public Constructors

            public ContentTreeServiceImpl(EventService eventService) : base(eventService)
            {
            }

            #endregion

            #region Protected Methods

            protected override Guid InternalCreateNode(Guid? parent, string alias)
            {
                return Guid.Empty;
            }

            protected override IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id)
            {
                throw new NotImplementedException();
            }

            protected override T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id)
            {
                throw new NotImplementedException();
            }

            protected override void InternalSave<T>(Guid id, Guid nodeId, T item)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        internal class EventHandler : IEventHandler
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

        #endregion
    }
}