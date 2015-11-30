using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
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
            var service = new NpgsqlContentTreeProvider(Mock.Of<ILoggerFactory>());

            var originalParent = Guid.NewGuid();
            service.CreateNode(originalParent, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            var node = Guid.NewGuid();
            service.CreateNode(node, originalParent, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            var targetParent = Guid.NewGuid();
            service.CreateNode(targetParent, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

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
            var service = new NpgsqlContentTreeProvider(Mock.Of<ILoggerFactory>());

            var originalParent = Guid.NewGuid();
            service.CreateNode(originalParent, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            var node = Guid.NewGuid();
            service.CreateNode(node, originalParent, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);


            // Act & Assert
            Assert.Throws<global::Npgsql.NpgsqlException>(() =>
            {
                service.MoveNode(node, Guid.Empty);
            });
        }

        #endregion
    }
}