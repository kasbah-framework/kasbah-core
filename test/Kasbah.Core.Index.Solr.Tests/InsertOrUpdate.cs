using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kasbah.Core.ContentTree.Npgsql.Tests;
using Kasbah.Core.Models;
using Xunit;

namespace Kasbah.Core.Index.Solr.Tests
{
    public class InsertOrUpdate
    {
        [SolrDbFact]
        public void ActiveVersionSet_VersionIndexed()
        {
            // Arrange
            var eventService = new Kasbah.Core.Events.InProcEventService();
            var contentTreeService = new Kasbah.Core.ContentTree.Npgsql.ContentTreeService(eventService);
            var provider = new SolrIndexProvider();
            var service = new IndexService(provider, eventService, contentTreeService);

            var node = contentTreeService.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

            var version = contentTreeService.Save<TestItem>(Guid.NewGuid(), node, new TestItem { Value = Guid.NewGuid().ToString() });

            // Act
            contentTreeService.SetActiveNodeVersion(node, version.Id);

            // Assert
        }

        [SolrDbFact]
        public void Query_WhereEntryExists_ReturnsIndexEntry()
        {
            // Arrange
            var provider = new SolrIndexProvider();

            var value = Guid.NewGuid();

            provider.Store(new Dictionary<string, object> {
                { "id", Guid.NewGuid() },
                { "A", 1 },
                { "Value", value }
            });

            // Act
            Thread.Sleep(1100); // commit timeout 1000ms

            var results = provider.Query(new { Value = value });

            // Assert
            Assert.NotEmpty(results);
            Assert.Equal(value, results.First()["Value"]);
        }

        [SolrDbFact]
        public void Index_RegularObject_SuccessfullyIndexes()
        {
            // Arrange
            var provider = new SolrIndexProvider();

            // Act
            provider.Store(new Dictionary<string, object> {
                { "id", Guid.NewGuid() },
                { "A", 1 }
            });

            // Assert
        }

        class TestItem : ItemBase
        {
            public string Value { get; set; }
        }
    }
}