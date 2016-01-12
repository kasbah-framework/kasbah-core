using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.ContentTree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Models;
using Kasbah.Core.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Kasbah.Core.ContentBroker.Tests
{
    public class ItemBaseProxyTests
    {
        #region Public Methods

        [Fact]
        public void GetProp_ThatExists_ReturnsCorrectValue()
        {
            // Arrange
            var expected = "hello";
            var dict = new Dictionary<string, object> { { "string", expected } };
            var obj = new ItemBaseProxy(typeof(ExampleItem), dict, Utils.MockContentBroker()).GetTransparentProxy() as ExampleItem;

            // Act
            var actual = obj.String;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetProp_ReferencingOtherItem_HitsContentTreeService()
        {
            // Arrange
            var id = Guid.NewGuid();
            var versionId = Guid.NewGuid();
            var version = new CrossReferenceB();

            var expected = new Node
            {
                ActiveVersion = versionId,
                Type = typeof(CrossReferenceB).AssemblyQualifiedName
            };

            var provider = new Mock<IContentTreeProvider>();
            provider.Setup(e => e.GetNode(id)).Returns(expected).Verifiable();
            provider.Setup(e => e.GetNodeVersion(id, versionId)).Verifiable();

            var contentBroker = new ContentBroker(new ContentTreeService(provider.Object), new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), Mock.Of<ILoggerFactory>());
            var dict = new Dictionary<string, object> {
                { "a", "A" },
                { "b", id.ToString() }
            };

            // Act
            var obj = new ItemBaseProxy(typeof(CrossReferenceA), dict, contentBroker).GetTransparentProxy() as CrossReferenceA;

            var _ = obj.B;

            // Assert
            provider.Verify();
        }

        [Fact]
        public void GetProp_ReferencingOtherItems_HitsContentTreeService()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var versionId = Guid.NewGuid();
            var version = new CrossReferenceB();

            var expected1 = new Node
            {
                Id = id1,
                ActiveVersion = versionId,
                Type = typeof(CrossReferenceB).AssemblyQualifiedName
            };
            var expected2 = new Node
            {
                Id = id2,
                ActiveVersion = versionId,
                Type = typeof(CrossReferenceB).AssemblyQualifiedName
            };

            var provider = new Mock<IContentTreeProvider>();
            provider.Setup(e => e.GetNode(id1)).Returns(expected1).Verifiable();
            provider.Setup(e => e.GetNode(id2)).Returns(expected2).Verifiable();
            provider.Setup(e => e.GetNodeVersion(id1, versionId)).Verifiable();
            provider.Setup(e => e.GetNodeVersion(id2, versionId)).Verifiable();

            var contentBroker = new ContentBroker(new ContentTreeService(provider.Object), new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), Mock.Of<ILoggerFactory>());
            var dict = new Dictionary<string, object> {
                { "bs", JsonConvert.SerializeObject(new [] { id1, id2 }) }
            };

            // Act
            var obj = new ItemBaseProxy(typeof(CrossReferenceC), dict, contentBroker).GetTransparentProxy() as CrossReferenceC;

            var _0 = obj.Bs;
            var _1 = _0.First();
            var _2 = _0.Skip(1).First();

            // Assert
            provider.Verify();
        }

        #endregion
    }

    class CrossReferenceA : ItemBase
    {
        public string A { get; set; }

        public CrossReferenceB B { get; set; }
    }

    class CrossReferenceB : ItemBase
    {
        public string B { get; set; }
    }

    class CrossReferenceC : ItemBase
    {
        public IEnumerable<CrossReferenceB> Bs { get; set; }
    }
}