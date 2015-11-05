using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
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
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var alias = Guid.NewGuid().ToString();

            var parentNode = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());
            var childNode = service.CreateNode<EmptyItem>(parentNode, alias);

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
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var parentNode = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Act
            var childNode = service.GetChild(parentNode, Guid.NewGuid().ToString());

            // Assert
            Assert.Null(childNode);
        }

        #endregion
    }
}