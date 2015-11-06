using System;
using Kasbah.Core.ContentTree.Npgsql.Tests;
using Kasbah.Core.Models;

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
            var service = new SolrIndexService(eventService, contentTreeService);

            var node = contentTreeService.CreateNode<TestItem>(null, Guid.NewGuid().ToString());

            var version = contentTreeService.Save<TestItem>(Guid.NewGuid(), node, new TestItem { Value = Guid.NewGuid().ToString() });

            // Act
            contentTreeService.SetActiveNodeVersion(node, version.Id);

            // Assert
        }

        class TestItem : ItemBase
        {
            public string Value { get; set; }
        }
    }
}