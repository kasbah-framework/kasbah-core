using System;
using Kasbah.Core.Cache;
using Kasbah.Core.ContentBroker.Events;
using Kasbah.Core.ContentBroker.Models;
using Kasbah.Core.Tree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentBroker.Tests
{
    public class Create
    {
        #region Public Methods

        [Fact]
        public void Create_WithValidInfo_CallsMethodOnTreeProvider()
        {
            // Arrange
            var alias = Guid.NewGuid().ToString();
            var type = TypeUtil.TypeName<ExampleItem>();

            var provider = new Mock<ITreeProvider>();
            provider.Setup(e => e.CreateNode(It.IsAny<Guid>(), null, alias, type)).Verifiable();

            var contentTreeService = new TreeService(provider.Object);

            var service = new ContentBroker(contentTreeService, new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), Mock.Of<CacheService>(), Mock.Of<ILoggerFactory>());

            // Act
            service.CreateNode<ExampleItem>(null, alias);

            // Assert
            provider.Verify();
        }

        [Fact]
        public void Create_WithValidInfo_EmitsEvent()
        {
            // Arrange
            var alias = Guid.NewGuid().ToString();
            var type = TypeUtil.TypeName<ExampleItem>();

            var provider = new Mock<IEventBusProvider>();
            provider.Setup(e => e.Emit(It.IsAny<NodeCreated>())).Verifiable();

            var eventService = new EventService(provider.Object);

            var service = new ContentBroker(new TreeService(Mock.Of<ITreeProvider>()), new IndexService(Mock.Of<IIndexProvider>()), eventService, Mock.Of<CacheService>(), Mock.Of<ILoggerFactory>());

            // Act
            service.CreateNode(null, alias, type);

            // Assert
            provider.Verify();
        }

        #endregion
    }

    class ExampleItem : ItemBase
    {
        #region Public Properties

        public string String { get; set; }

        #endregion
    }
}