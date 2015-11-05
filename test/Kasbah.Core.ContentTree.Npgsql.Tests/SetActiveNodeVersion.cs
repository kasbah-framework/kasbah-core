using System;
using Kasbah.Core.Events;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_SetActiveNodeVersion
    {
        #region Public Methods

        [DbFact]
        public void SetActiveNodeVersion_WithExistingVersion_ActiveVersionUpdated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var id = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());
            var version = service.Save(Guid.NewGuid(), id, new TestItem());

            // Act
            service.SetActiveNodeVersion(id, version.Id);

            var node = service.GetNode(id);

            // Assert
            Assert.Equal(node.ActiveVersionId, version.Id);
        }

        [DbFact]
        public void SetActiveNodeVersion_WithNonExistantVersion_ExceptionThrown()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var id = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());
            var version = service.Save(Guid.NewGuid(), id, new TestItem());

            // Act & Assert
            Assert.Throws<global::Npgsql.NpgsqlException>(() =>
            {
                service.SetActiveNodeVersion(id, Guid.Empty);
            });
        }

        #endregion
    }
}