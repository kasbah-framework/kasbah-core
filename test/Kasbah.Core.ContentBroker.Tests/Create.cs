using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kasbah.Core.ContentBroker.Events;
using Kasbah.Core.ContentTree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Models;
using Kasbah.Core.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Kasbah.Core.ContentBroker.Tests
{
    public class Create
    {
        [Fact]
        public void Create_WithValidInfo_EmitsEvent()
        {
            // Arrange
            var alias = Guid.NewGuid().ToString();
            var type = TypeUtil.TypeName<ExampleItem>();

            var provider = new Mock<IEventBusProvider>();
            provider.Setup(e => e.Emit(It.IsAny<NodeCreated>())).Verifiable();

            var eventService = new EventService(provider.Object);

            var service = new ContentBroker(new ContentTreeService(Mock.Of<IContentTreeProvider>()), new IndexService(Mock.Of<IIndexProvider>()), eventService, Mock.Of<ILoggerFactory>());

            // Act
            service.CreateNode(null, alias, type);

            // Assert
            provider.Verify();
        }

        [Fact]
        public void Create_WithValidInfo_CallsMethodOnTreeProvider()
        {
            // Arrange
            var alias = Guid.NewGuid().ToString();
            var type = TypeUtil.TypeName<ExampleItem>();

            var provider = new Mock<IContentTreeProvider>();
            provider.Setup(e => e.CreateNode(It.IsAny<Guid>(), null, alias, type)).Verifiable();

            var contentTreeService = new ContentTreeService(provider.Object);

            var service = new ContentBroker(contentTreeService, new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), Mock.Of<ILoggerFactory>());

            // Act
            service.CreateNode<ExampleItem>(null, alias);

            // Assert
            provider.Verify();
        }
    }

    class ExampleItem : ItemBase
    {
        public string String { get; set; }
    }
}
