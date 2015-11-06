using System;
using System.Threading;
using Kasbah.Core.ContentTree.Npgsql.Tests;
using Kasbah.Core.Models;
using Xunit;

namespace Kasbah.Core.Index.Solr.Tests
{
    public class Query
    {
        [SolrDbFact]
        public void Query_DoesNotThrowException()
        {
            // Arrange
            var eventService = new Kasbah.Core.Events.InProcEventService();
            var contentTreeService = new Kasbah.Core.ContentTree.Npgsql.ContentTreeService(eventService);
            var service = new SolrIndexService(eventService, contentTreeService);

            var node = contentTreeService.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

            var value = Guid.NewGuid().ToString();

            var version = contentTreeService.Save<TestItem>(Guid.NewGuid(), node, new TestItem { Value = value });
            contentTreeService.SetActiveNodeVersion(node, version.Id);

            // Insert is given 1000ms to commit
            Thread.Sleep(1100);

            // Act
            var results = service.Query(value);

            // Assert
            Assert.NotEmpty(results);
        }

        class TestItem : ItemBase
        {
            public string Value { get; set; }
        }
    }
}