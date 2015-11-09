using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Tests.TestImpls;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class CreateNode
    {
        [Fact]
        public void CreateNode_TriggersBeforeAndAfterCreateEvents_EventsTriggered()
        {
            // Arrange
            var eventService = new InProcEventService();
            var handler = new BasicEventHandler();
            var provider = Mock.Of<IContentTreeProvider>();

            eventService.Register<BeforeNodeCreated>(handler);
            eventService.Register<AfterNodeCreated>(handler);

            var service = new ContentTreeService(provider, eventService);

            // Act
            service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEmpty(handler.HandledEvents);
            Assert.Equal(2, handler.HandledEvents.Count);
        }
    }
}