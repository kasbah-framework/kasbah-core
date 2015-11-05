using System;
using System.Linq;
using System.Threading;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_Save
    {
        #region Public Methods


        [DbFact]
        public void Save_ExistingItem_ItemUpdated()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

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

            var node = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

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

            var node = service.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

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

        [DbFact]
        public void Save_AnonymousObject_ItemSaved()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode<EmptyItem>(null, Guid.NewGuid().ToString());

            // Act
            var id = Guid.NewGuid();
            var item = new { Value = "test" };

            var savedItem = service.Save(id, node, (object)item);

            var outItem = service.GetNodeVersion(node, id);

            // Assert
            Assert.NotNull(savedItem);
            Assert.NotNull(outItem);
            Assert.Equal(item.Value, outItem["value"]);
        }

        #endregion
    }
}