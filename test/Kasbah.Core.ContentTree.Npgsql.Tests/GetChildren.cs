using System;
using System.Linq;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
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
            var service = new NpgsqlContentTreeProvider(Mock.Of<ILoggerFactory>());

            var parentNode = Guid.NewGuid();
            service.CreateNode(parentNode, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            // Act
            var createdChildNodeIds = new[] {
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            foreach (var id in createdChildNodeIds)
            {
                service.CreateNode(id, parentNode, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);
            }

            var actualChildNodes = service.GetChildren(parentNode);

            // Assert
            Assert.NotEmpty(actualChildNodes);
            Assert.Contains(createdChildNodeIds[0], actualChildNodes.Select(ent => ent.Id));
            Assert.Contains(createdChildNodeIds[1], actualChildNodes.Select(ent => ent.Id));
        }

        [DbFact]
        public void GetChildren_WhereNodeHasNoChildren_ReturnsNoChildNodes()
        {
            // Arrange
            var service = new NpgsqlContentTreeProvider(Mock.Of<ILoggerFactory>());

            var parentNode = Guid.NewGuid();
            service.CreateNode(parentNode, null, Guid.NewGuid().ToString(), typeof(EmptyItem).FullName);

            // Act
            var childNodes = service.GetChildren(parentNode);

            // Assert
            Assert.Empty(childNodes);
        }

        #endregion
    }
}