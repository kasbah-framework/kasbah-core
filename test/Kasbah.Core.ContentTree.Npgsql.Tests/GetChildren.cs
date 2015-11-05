using System;
using System.Linq;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_GetChildren
    {
        #region Public Methods
        [DbFact]
        public void GetChildren_WhereNodeHasChildren_ReturnsChildNodes()
        {
            // Arrange
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var parentNode = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Act
            var createdChildNodes = new[] {
                service.CreateNode<EmptyItem>(parentNode, Guid.NewGuid().ToString()),
                service.CreateNode<EmptyItem>(parentNode, Guid.NewGuid().ToString())
            };

            var actualChildNodes = service.GetChildren(parentNode);

            // Assert
            Assert.NotEmpty(actualChildNodes);
            Assert.Contains(createdChildNodes[0], actualChildNodes.Select(ent => ent.Id));
            Assert.Contains(createdChildNodes[1], actualChildNodes.Select(ent => ent.Id));
        }

        [DbFact]
        public void GetChildren_WhereNodeHasNoChildren_ReturnsNoChildNodes()
        {
            // Arrange
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var parentNode = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Act
            var childNodes = service.GetChildren(parentNode);

            // Assert
            Assert.Empty(childNodes);
        }

        #endregion
    }
}