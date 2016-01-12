using System.Collections.Generic;
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

        // TODO: test node to node referencing

        #endregion
    }
}