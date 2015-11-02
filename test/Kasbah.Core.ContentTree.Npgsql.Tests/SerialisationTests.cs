using Kasbah.Core.ContentTree.Npgsql.Utils;
using Xunit;

namespace Kasbah.Core.ContentTree.Tests
{
    public class SerialisationTests
    {
        #region Public Methods

        [Fact]
        public void Deserialise_BasicString_ReturnsString()
        {
            // Arrange
            var input = "'string'";
            var expected = "string";

            // Act
            var output = SerialisationUtil.Deserialise<string>(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void Serialise_BasicObject_ReturnsCorrectJson()
        {
            // Arrange
            var obj = new { a = 1 };
            var expected = "{\"a\":1}";

            // Act
            var output = SerialisationUtil.Serialise(obj);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void Serialisation_DeserialiseToDictionary_ReturnsValidDictionary()
        {
            // Arrange
            const string Input = "{\"a\":1, \"b\":\"b\"}";

            // Act
            var output = SerialisationUtil.DeserialiseAnonymous(Input);

            // Assert
            Assert.Equal((long)1, output["a"]);
            Assert.Equal("b", output["b"]);
        }

        #endregion
    }
}