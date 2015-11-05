using System;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Tests.TestImpls;
using Kasbah.Core.Events;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class ItemOperations
    {
        #region Public Methods

        [Fact]
        public void SaveItem_TriggersBeforeAndAfterEvents_EventsTriggered()
        {
            // Arrange
            var eventService = new InProcEventService();
            var handler = new BasicEventHandler();

            eventService.Register<BeforeItemSaved>(handler);
            eventService.Register<AfterItemSaved>(handler);

            var service = new ContentTreeService(eventService);


            var node = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

            // Act
            service.Save(Guid.NewGuid(), node, new TestItem());

            // Assert
            Assert.NotEmpty(handler.HandledEvents);

            Assert.Equal(2, handler.HandledEvents.Count);
        }

        #endregion
    }
}