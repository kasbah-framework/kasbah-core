using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_MoveNode
    {
        #region Public Methods

        [DbFact]
        public void MoveNode_WhenTargetExists_NodeMoved()
        {
            // Arrange
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var originalParent = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());
            var node = service.CreateNode<EmptyItem>(originalParent, Guid.NewGuid().ToString());
            var targetParent = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Act
            service.MoveNode(node, targetParent);

            var movedNode = service.GetNode(node);

            // Assert
            Assert.Equal(targetParent, movedNode.Parent);
        }

        [DbFact]
        public void MoveNode_WhenTargetDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var originalParent = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());
            var node = service.CreateNode<EmptyItem>(originalParent, Guid.NewGuid().ToString());

            // Act & Assert
            Assert.Throws<global::Npgsql.NpgsqlException>(() =>
            {
                service.MoveNode(node, Guid.Empty);
            });
        }

        #endregion
    }
}