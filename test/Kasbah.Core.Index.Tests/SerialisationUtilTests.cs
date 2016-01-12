using System;
using Kasbah.Core.Index.Attributes;
using Kasbah.Core.Index.Utils;
using Xunit;

namespace Kasbah.Core.Index.Tests
{
    public class SerialisationUtilTests
    {
        #region Public Methods

        [Fact]
        public void Serialise_ObjectWithIgnoredProperty_DoesNotContainIgnoredProperty()
        {
            // Arrange
            var obj = new TypeWithIgnoredProperty { A = "A", B = "B" };

            // Act
            var dict = SerialisationUtil.Serialise(obj);

            // Assert
            Assert.True(dict.ContainsKey("A"));
            Assert.False(dict.ContainsKey("B"));
        }

        [Fact]
        public void Serialise_WithNullInput_ThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                SerialisationUtil.Serialise(null);
            });
        }

        [Fact]
        public void Serialise_WithRegularObject_ReturnsPopulatedDictionary()
        {
            // Arrange
            var obj = new { A = 1, B = "C" };

            // Act
            var dict = SerialisationUtil.Serialise(obj);

            // Assert
            Assert.NotNull(dict);
            Assert.True(dict.ContainsKey("A"));
            Assert.True(dict.ContainsKey("B"));
            Assert.Equal("C", dict["B"]);
        }

        #endregion

        #region Private Classes

        class TypeWithIgnoredProperty
        {
            #region Public Properties

            public string A { get; set; }

            [IndexIgnore]
            public string B { get; set; }

            #endregion
        }

        #endregion
    }
}