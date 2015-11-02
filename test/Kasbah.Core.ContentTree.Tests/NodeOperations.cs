using System;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Tests.TestImpls;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
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
            var eventService = new InProcEventService();
            var handler = new BasicEventHandler();

            eventService.Register<BeforeNodeCreated>(handler);
            eventService.Register<AfterNodeCreated>(handler);

            var service = new ContentTreeServiceNoop(eventService);

            // Act
            service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEmpty(handler.HandledEvents);

            Assert.Equal(2, handler.HandledEvents.Count);
        }

        #endregion
    }
}