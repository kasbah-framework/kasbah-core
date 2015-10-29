using System;
using System.Threading;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class NodeOperations
    {
        [DbFact]
        public void NpgsqlContentTree_CreateNode_NodeCreated()
        {
            // Arrange
            var eventService = new EventService();
            var service = new ContentTreeService(eventService);

            // Act
            var id = service.CreateNode(null, Guid.NewGuid().ToString());

            // Assert
            Assert.NotEqual(Guid.Empty, id);
        }

        [DbFact]
        public void NpgsqlContentTree_SaveNewItem_ItemSaved()
        {
            // Arrange
            var eventService = new EventService();
            var service = new ContentTreeService(eventService);

            var node = service.CreateNode(null, Guid.NewGuid().ToString());

            // Act
            var id = Guid.NewGuid();
            var item = new TestItem { Value = "test" };

            var savedItem = service.Save(id, node, item);

            // Assert
            Assert.NotNull(savedItem);
        }

        [DbFact]
        public void NpgsqlContentTree_SaveExistingItem_ItemUpdated()
        {
            // Arrange
            var eventService = new EventService();
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
    }

    internal class TestItem : ItemBase
    {
        public string Value { get; set; }
    }

    public class DbFactAttribute : FactAttribute
    {
        public DbFactAttribute()
        {
            if (Environment.GetEnvironmentVariable("DB") == null)
            {
                Skip = "Database unavailable";
            }
        }
    }
}