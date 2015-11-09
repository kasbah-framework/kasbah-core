using System;
using Kasbah.Core.Events;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_GetActiveNodeVersion
    {
        #region Public Methods

        [DbFact]
        public void GetActiveNodeVersion_WithActiveVersion_ReturnsCorrectVersion()
        {
            var eventService = Mock.Of<IEventService>();
            var service = new NpgsqlContentTreeProvider();

            var inItem = new TestItem { Value = Guid.NewGuid().ToString() };

            var id = Guid.NewGuid();
            service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(TestItem).FullName);
            var version = service.Save(Guid.NewGuid(), id, inItem);

            service.SetActiveNodeVersion(id, version.Id);

            // Act
            var activeVersion = service.GetActiveNodeVersion<TestItem>(id);

            // Assert
            Assert.NotNull(activeVersion);
            Assert.Equal(inItem.Value, activeVersion.Value);
        }

        [DbFact]
        public void GetActiveNodeVersion_WithNoActiveVersion_ReturnsNull()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new NpgsqlContentTreeProvider();

            var id = Guid.NewGuid();
            service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(TestItem).FullName);

            // Act
            var version = service.GetActiveNodeVersion<TestItem>(id);

            // Assert
            Assert.Null(version);
        }

        #endregion
    }
}