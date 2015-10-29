using System;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Tests.TestImpls;
using Kasbah.Core.Events;
using Xunit;

namespace Kasbah.Core.ContentTree.Tests
{
    public class ItemOperations
    {
        #region Public Methods

        [Fact]
        public void SaveItem_TriggersBeforeAndAfterEvents_EventsTriggered()
        {
            // Arrange
            var eventService = new EventService();
            var handler = new BasicEventHandler();

            eventService.Register<BeforeItemSaved>(handler);
            eventService.Register<AfterItemSaved>(handler);

            var service = new ContentTreeServiceNoop(eventService);

            // Act
            service.Save(Guid.Empty, Guid.Empty, new ItemBaseImpl());

            // Assert
            Assert.NotEmpty(handler.HandledEvents);

            Assert.Equal(2, handler.HandledEvents.Count);
        }

        #endregion
    }
}