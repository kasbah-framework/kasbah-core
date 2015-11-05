using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_GetNode
    {
        #region Public Methods

        [DbFact]
        public void GetNode_WhereNodeExists_ReturnsCorrectNode()
        {
            // Arrange
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var id = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Act
            var node = service.GetNode(id);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(id, node.Id);
        }

        [DbFact]
        public void GetNode_WhereNodeDoesNotExists_ReturnsNull()
        {
            // Arrange
            var service = new ContentTreeService(Mock.Of<IEventService>());

            // Act
            var node = service.GetNode(Guid.Empty);

            // Assert
            Assert.Null(node);
        }

        #endregion
    }
}