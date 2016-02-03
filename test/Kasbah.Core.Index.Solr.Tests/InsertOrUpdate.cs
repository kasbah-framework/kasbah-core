using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kasbah.Core.Index.Solr.Tests
{
    public class InsertOrUpdate
    {
        #region Public Methods

        [SolrDbFact]
        public void Index_RegularObject_SuccessfullyIndexes()
        {
            // Arrange
            var provider = new SolrIndexProvider(Mock.Of<ILoggerFactory>());

            // Act
            provider.Store(new Dictionary<string, object> {
                 { "id", Guid.NewGuid() },
                 { "A", 1 }
             });

            // Assert
        }

        [SolrDbFact]
        public void Query_WhereEntryExists_ReturnsIndexEntry()
        {
            // Arrange
            var provider = new SolrIndexProvider(Mock.Of<ILoggerFactory>());

            var id = Guid.NewGuid();
            const int aValue = 1;

            provider.Store(new Dictionary<string, object> {
                 { "id", id },
                 { "A", aValue }
             });

            // Act
            Thread.Sleep(1100); // commit timeout 1000ms

            var results = provider.Query(new { id });

            // Assert
            Assert.NotEmpty(results);
            Assert.Equal(aValue, results.First()["A"]);
        }

        #endregion

        #region Private Classes

        class TestItem
        {
            #region Public Properties

            public string Value { get; set; }

            #endregion
        }

        #endregion
    }
}