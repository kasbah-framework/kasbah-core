using System;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kasbah.Core.Tree.Npgsql.Tests
{
    public class Npgsql_GetNode
    {
        #region Public Methods

        [DbFact]
        public void GetNode_WhereNodeDoesNotExists_ReturnsNull()
        {
            // Arrange
            var service = new NpgsqlTreeProvider(Mock.Of<ILoggerFactory>());

            // Act
            var node = service.GetNode(Guid.Empty);

            // Assert
            Assert.Null(node);
        }

        [DbFact]
        public void GetNode_WhereNodeExists_ReturnsCorrectNode()
        {
            // Arrange
            var service = new NpgsqlTreeProvider(Mock.Of<ILoggerFactory>());

            var id = Guid.NewGuid();
            service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            // Act
            var node = service.GetNode(id);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(id, node.Id);
        }

        #endregion
    }
}