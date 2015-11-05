using System;
using Kasbah.Core.Events;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_GetNodeVersion
    {
        #region Public Methods

        [DbFact]
        public void GetNodeVersion_WhereVersionExists_ReturnsCorrectVersion()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var item = new TestItem { Value = Guid.NewGuid().ToString() };

            var id = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());
            var version = service.Save<TestItem>(Guid.NewGuid(), id, item);

            // Act
            var outItem = service.GetNodeVersion<TestItem>(id, version.Id);

            // Assert
            Assert.NotNull(outItem);
            Assert.Equal(item.Value, outItem.Value);
        }

        [DbFact]
        public void GetNodeVersion_WhereVersionDoesNotExist_ReturnsNull()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var id = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

            // Act
            var outItem = service.GetNodeVersion<TestItem>(id, Guid.Empty);

            // Assert
            Assert.Null(outItem);
        }

        [DbFact]
        public void GetNodeVersion_WithoutType_ReturnsValidDictionary()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var item = new TestItem { Value = Guid.NewGuid().ToString() };

            var id = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());
            var version = service.Save<TestItem>(Guid.NewGuid(), id, item);

            // Act
            var outItem = service.GetNodeVersion(id, version.Id);

            // Assert
            Assert.NotNull(outItem);
            Assert.True(outItem.ContainsKey("value"));
            Assert.Equal(item.Value, outItem["value"]);
        }

        [DbFact]
        public void GetNodeVersion_WithoutGenericType_ReturnsValidObject()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var item = new TestItem { Value = Guid.NewGuid().ToString() };

            var id = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());
            var version = service.Save<TestItem>(Guid.NewGuid(), id, item);

            var type = typeof(TestItem);

            // Act
            var outItem = service.GetNodeVersion(id, version.Id, type) as TestItem;

            // Assert
            Assert.NotNull(outItem);
            Assert.Equal(item.Value, outItem.Value);
        }

        #endregion
    }
}