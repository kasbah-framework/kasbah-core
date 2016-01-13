using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.ContentTree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Kasbah.Core.ContentBroker.Tests
{
    public class ItemBaseProxyTests
    {
        // TODO: outstanding tests
        // tests caching of values

        #region Public Methods

        [Fact]
        public void CallMethod_ThatThrowsAnException_ReturnsTheSameException()
        {
            // Arrange
            var dict = new Dictionary<string, object> { };
            var obj = new ItemBaseProxy(typeof(ItemWithMethodThatThrowsException), dict, Utils.MockContentBroker()).GetTransparentProxy() as ItemWithMethodThatThrowsException;

            // Act & Assert
            Assert.Throws<CustomException>(() => obj.ExceptionThrowingMethod(new CustomException()));
        }

        [Fact]
        public void GetNode_WhereNodeExists_ReturnsNode()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expected = new Node { };

            var provider = new Mock<IContentTreeProvider>();
            provider.Setup(e => e.GetNode(id)).Returns(expected);

            var contentBroker = new ContentBroker(new ContentTreeService(provider.Object), new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), Mock.Of<ILoggerFactory>());
            var dict = new Dictionary<string, object> {
                { "id", id.ToString() }
            };

            var obj = new ItemBaseProxy(typeof(CrossReferenceA), dict, contentBroker).GetTransparentProxy() as CrossReferenceA;

            // Act
            var node = obj.Node;

            // Assert
            Assert.NotNull(node);
            Assert.Equal(expected, node);
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

            var actual = obj.B;

            // Assert
            provider.Verify();
            Assert.NotNull(actual);
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
            Assert.NotNull(_0);
            Assert.NotNull(_1);
            Assert.NotNull(_2);
        }

        [Fact]
        public void GetProp_ReferencingOtherItemWithNoActiveVersion_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expected = new Node
            {
                ActiveVersion = null,
                Type = typeof(CrossReferenceB).AssemblyQualifiedName
            };

            var provider = new Mock<IContentTreeProvider>();
            provider.Setup(e => e.GetNode(id)).Returns(expected).Verifiable();

            var contentBroker = new ContentBroker(new ContentTreeService(provider.Object), new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), Mock.Of<ILoggerFactory>());
            var dict = new Dictionary<string, object> {
                { "a", "A" },
                { "b", id.ToString() }
            };

            // Act
            var obj = new ItemBaseProxy(typeof(CrossReferenceA), dict, contentBroker).GetTransparentProxy() as CrossReferenceA;

            var actual = obj.B;

            // Assert
            provider.Verify();
            Assert.Null(actual);
        }

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
        public void GetProp_TwoTimes_ReturnsCachedValue()
        {
            // Arrange
            var dict = new Dictionary<string, object> { { "a", "A" } };
            var obj = new ItemBaseProxy(typeof(CrossReferenceA), dict, Utils.MockContentBroker()).GetTransparentProxy() as CrossReferenceA;

            // Act
            var actual1 = obj.A;
            var actual2 = obj.A;

            // Assert
            Assert.Same(actual1, actual2);
        }

        [Fact]
        public void OnProxiedItem_CallMethod_ReturnsActualMethodResponse()
        {
            // Arrange
            var dict = new Dictionary<string, object> { };
            var obj = new ItemBaseProxy(typeof(ItemWithMethod), dict, Utils.MockContentBroker()).GetTransparentProxy() as ItemWithMethod;

            // Act
            var actual = obj.MyMethod();

            // Assert
            Assert.Equal(ItemWithMethod.Expected, actual);
        }

        [Fact]
        public void OnProxiedItem_GetPropertyNotFromDict_ReturnsActualPropertyValue()
        {
            // Arrange
            var dict = new Dictionary<string, object> { };
            var obj = new ItemBaseProxy(typeof(ItemWithPropNotInDict), dict, Utils.MockContentBroker()).GetTransparentProxy() as ItemWithPropNotInDict;

            // Act
            var actual = obj.NotInDict;

            // Assert
            Assert.Equal(ItemWithPropNotInDict.Expected, actual);
        }

        #endregion
    }

    class CrossReferenceA : ItemBase
    {
        #region Public Properties

        public string A { get; set; }

        public CrossReferenceB B { get; set; }

        #endregion
    }

    class CrossReferenceB : ItemBase
    {
        #region Public Properties

        public string B { get; set; }

        #endregion
    }

    class CrossReferenceC : ItemBase
    {
        #region Public Properties

        public IEnumerable<CrossReferenceB> Bs { get; set; }

        #endregion
    }

    class CustomException : Exception { }

    class ItemWithMethod : ItemBase
    {
        #region Public Fields

        public const string Expected = nameof(ItemWithMethod);

        #endregion

        #region Public Methods

        public string MyMethod()
        {
            return Expected;
        }

        #endregion
    }

    class ItemWithMethodThatThrowsException : ItemBase
    {
        #region Public Methods

        public void ExceptionThrowingMethod(Exception ex)
        {
            throw ex;
        }

        #endregion
    }

    class ItemWithPropNotInDict : ItemBase
    {
        #region Public Fields

        public const string Expected = nameof(ItemWithPropNotInDict);

        #endregion

        #region Public Properties

        public string NotInDict { get; set; } = nameof(ItemWithPropNotInDict);

        #endregion
    }
}