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
            var service = new NpgsqlContentTreeProvider();

            var id = Guid.NewGuid();
            service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(TestItem).FullName);
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
            var service = new NpgsqlContentTreeProvider();

            var id = Guid.NewGuid();
            service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(TestItem).FullName);
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