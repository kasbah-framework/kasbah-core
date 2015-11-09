using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Tests.TestImpls;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Save
    {
        [Fact]
        public void Save_TriggersBeforeAndAfterEvents_EventsTriggered()
        {
            // Arrange
            var eventService = new InProcEventService();
            var handler = new BasicEventHandler();

            eventService.Register<BeforeItemSaved>(handler);
            eventService.Register<AfterItemSaved>(handler);

            var service = new ContentTreeService(Mock.Of<IContentTreeProvider>(), eventService);

            var node = service.CreateNode(null, Guid.NewGuid().ToString(), typeof(EmptyItem));

            // Act
            service.Save(Guid.NewGuid(), node, new EmptyItem());

            // Assert
            Assert.NotEmpty(handler.HandledEvents);
            Assert.Equal(2, handler.HandledEvents.Count);
        }
    }
}