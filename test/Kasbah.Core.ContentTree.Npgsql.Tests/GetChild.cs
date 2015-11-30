using System;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_GetChild
    {
        #region Public Methods

        [DbFact]
        public void GetChild_WhereChildExists_ReturnsChildNode()
        {
            // Arrange
            var service = new NpgsqlContentTreeProvider(Mock.Of<ILoggerFactory>());

            var alias = Guid.NewGuid().ToString();

            var parentNode = Guid.NewGuid();
            service.CreateNode(parentNode, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            var childNode = Guid.NewGuid();
            service.CreateNode(childNode, parentNode, alias, typeof(EmptyItem).FullName);

            // Act
            var outChildNode = service.GetChild(parentNode, alias);

            // Assert
            Assert.NotNull(outChildNode);
            Assert.Equal(childNode, outChildNode.Id);
        }

        [DbFact]
        public void GetChild_WhereNoChildExists_ReturnsNull()
        {
            // Arrange
            var service = new NpgsqlContentTreeProvider(Mock.Of<ILoggerFactory>());

            var parentNode = Guid.NewGuid();
            service.CreateNode(parentNode, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            // Act
            var childNode = service.GetChild(parentNode, Guid.NewGuid().ToString());

            // Assert
            Assert.Null(childNode);
        }

        #endregion
    }
}