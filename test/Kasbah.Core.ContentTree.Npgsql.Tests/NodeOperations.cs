using System;
using System.Linq;
using System.Threading;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Tests.TestImpls;
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
        public void CreateNewNode_WithCorrectProperties_NodeCreated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            // Act
            var id = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEqual(Guid.Empty, id);
        }

        [DbFact]
        public void CreateNode_TriggersBeforeAndAfterCreateEvents_EventsTriggered()
        {
            // Arrange
            var eventService = new InProcEventService();
            var handler = new BasicEventHandler();

            eventService.Register<BeforeNodeCreated>(handler);
            eventService.Register<AfterNodeCreated>(handler);

            var service = new ContentTreeService(eventService);

            // Act
            service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEmpty(handler.HandledEvents);
            Assert.Equal(2, handler.HandledEvents.Count);
        }

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

        [DbFact]
        public void Save_ExistingItem_ItemUpdated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

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
        public void Save_NewItem_ItemSaved()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Act
            var id = Guid.NewGuid();
            var item = new TestItem { Value = "test" };

            var savedItem = service.Save(id, node, item);

            // Assert
            Assert.NotNull(savedItem);
        }

        [DbFact]
        public void Save_WithUniqueIds_MultipleVersionCreated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

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