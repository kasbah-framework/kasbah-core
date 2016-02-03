using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kasbah.Core.Index.Solr.Tests
{
    public class Delete
    {
        #region Public Methods

        [SolrDbFact]
        public void Delete_IndexedObject_SuccessfullyRemovesFromIndex()
        {
            // Arrange
            var provider = new SolrIndexProvider(Mock.Of<ILoggerFactory>());
            var id = Guid.NewGuid();

            provider.Store(new Dictionary<string, object> {
                 { "id", id },
                 { "A", 1 }
             });
            Thread.Sleep(1100); // commit timeout 1000ms

            // Act
            provider.Delete(id);
            Thread.Sleep(1100); // commit timeout 1000ms

            var result = provider.Query(new { id });

            // Assert
            Assert.Empty(result);
        }

        #endregion
    }
}