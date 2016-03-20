using System;
using Kasbah.Core.Cache;
using Kasbah.Core.Tree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentBroker.Tests
{
    public class Caching
    {
        #region Public Methods

        [Fact]
        public void CreateNode_InvalidatesCorrectCacheKeys()
        {
            // Arrange
            var alias = Guid.NewGuid().ToString();
            var type = TypeUtil.TypeName<ExampleItem>();

            var provider = new Mock<IDistributedCache>();
            provider.Setup(e => e.Remove(ContentBroker.CacheKeys.Children(null))).Verifiable();

            var cacheService = new CacheService(provider.Object);

            var service = MockContentBroker(cacheService);

            // Act
            service.CreateNode<ExampleItem>(null, alias);

            // Assert
            provider.Verify();
        }

        #endregion

        static ContentBroker MockContentBroker(CacheService cacheService)
            => new ContentBroker(new TreeService(Mock.Of<ITreeProvider>()), new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), cacheService, Mock.Of<ILoggerFactory>());
    }
}