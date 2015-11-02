using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class NodeOperations
    {
        #region Public Methods

        [DbFact]
        public void GetChildren_WhereNodeHasChildren_ReturnsChildNodes()
        {
            // Arrange
            var service = new ContentTreeService(Mock.Of<IEventService>());

            var parentNode = service.CreateNode(null, Guid.NewGuid().ToString());

            // Act
            var createdChildNodes = new[] {
                service.CreateNode(parentNode, Guid.NewGuid().ToString()),
                service.CreateNode(parentNode, Guid.NewGuid().ToString())
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

            var parentNode = service.CreateNode(null, Guid.NewGuid().ToString());

            // Act
            var childNodes = service.GetChildren(parentNode);

            // Assert
            Assert.Empty(childNodes);
        }

        [Fact]
        public void GetMostRecentlyCreatedVersion_WhenMultipleVersionsExist_ReturnsLatestVersion()
        {
            // TODO: this assertion could be cleaner

            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode(null, Guid.NewGuid().ToString());

            // Act
            var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
            var item = new TestItem { Value = Guid.NewGuid().ToString() };
            var values = new Dictionary<Guid, string>();

            foreach (var id in ids)
            {
                values[id] = Guid.NewGuid().ToString();

                service.Save(id, node, new TestItem { Value = values[id] });
                Thread.Sleep(5);
            }

            var latest = service.GetMostRecentlyCreatedNodeVersion<TestItem>(node);

            // Assert
            Assert.Equal(latest.Value, values[ids.Last()]);
        }

        [DbFact]
        public void NpgsqlContentTree_CreateNode_NodeCreated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            // Act
            var id = service.CreateNode(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEqual(Guid.Empty, id);
        }

        [DbFact]
        public void NpgsqlContentTree_SaveExistingItem_ItemUpdated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode(null, Guid.NewGuid().ToString());

            // Act
            var id = Guid.NewGuid();
            var item = new TestItem { Value = "test" };

            var firstSave = service.Save(id, node, item);
            Thread.Sleep(5); // sleep so modified time is updated
            var secondSave = service.Save(id, node, item);

            // Assert
            Assert.NotNull(firstSave);
            Assert.NotNull(secondSave);
            Assert.NotEqual(firstSave.Modified, secondSave.Modified);
            Assert.Equal(firstSave.Created, secondSave.Created);
            Assert.Equal(firstSave.Id, secondSave.Id);
        }

        [DbFact]
        public void NpgsqlContentTree_SaveNewItem_ItemSaved()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode(null, Guid.NewGuid().ToString());

            // Act
            var id = Guid.NewGuid();
            var item = new TestItem { Value = "test" };

            var savedItem = service.Save(id, node, item);

            // Assert
            Assert.NotNull(savedItem);
        }

        [Fact]
        public void Save_WithUniqueIds_MultipleVersionCreated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode(null, Guid.NewGuid().ToString());

            // Act
            var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };
            var item = new TestItem { Value = Guid.NewGuid().ToString() };

            foreach (var id in ids)
            {
                service.Save(id, node, item);
            }

            var versions = service.GetAllNodeVersions(node);

            // Assert
            Assert.NotEmpty(versions);
            Assert.Equal(ids, versions.Select(ent => ent.Id));
        }

        #endregion
    }

    internal class TestItem : ItemBase
    {
        #region Public Properties

        public string Value { get; set; }

        #endregion
    }
}