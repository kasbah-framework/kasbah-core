using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Tests
{
    public class Save
    {
        #region Public Methods

        [Fact]
        public void Noop()
        {
            var service = new ContentTreeService(Mock.Of<IContentTreeProvider>());
        }

        #endregion

        //[Fact]
        //public void Save_TriggersBeforeAndAfterEvents_EventsTriggered()
        //{
        //    // Arrange
        //    var eventService = new InProcEventService();
        //    var handler = new BasicEventHandler();

        //    eventService.Register<BeforeItemSaved>(handler);
        //    eventService.Register<AfterItemSaved>(handler);

        //    var service = new ContentTreeService(Mock.Of<IContentTreeProvider>(), eventService);

        //    var node = service.CreateNode(null, Guid.NewGuid().ToString(), typeof(EmptyItem));

        //    // Act
        //    service.Save(Guid.NewGuid(), node, new EmptyItem());

        //    // Assert
        //    Assert.NotEmpty(handler.HandledEvents);
        //    Assert.Equal(2, handler.HandledEvents.Count);
        //}
    }
}