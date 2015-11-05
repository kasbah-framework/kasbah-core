using System;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Tests.TestImpls;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_CreateNode
    {
        #region Public Methods

        [DbFact]
        public void CreateNode_WithCorrectProperties_NodeCreated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            // Act
            var id = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEqual(Guid.Empty, id);
        }

        [DbFact]
        public void CreateNode_TriggersBeforeAndAfterCreateEvents_EventsTriggered()
        {
            // Arrange
            var eventService = new InProcEventService();
            var handler = new BasicEventHandler();

            eventService.Register<BeforeNodeCreated>(handler);
            eventService.Register<AfterNodeCreated>(handler);

            var service = new ContentTreeService(eventService);

            // Act
            service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEmpty(handler.HandledEvents);
            Assert.Equal(2, handler.HandledEvents.Count);
        }

        [DbFact]
        public void CreateNode_WeaklyTyped_NodeCreated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            // Act
            var id = service.CreateNode(null, Guid.NewGuid().ToString(), nameof(CreateNode_WeaklyTyped_NodeCreated));

            // Assert
            Assert.NotEqual(Guid.Empty, id);
        }

        #endregion
    }
}