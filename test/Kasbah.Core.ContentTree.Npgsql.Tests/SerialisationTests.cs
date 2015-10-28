using System.Text;
using Xunit;
using Kasbah.Core.ContentTree.Npgsql.Utils;

namespace Kasbah.Core.ContentTree.Tests
{
    public class SerialisationTests
    {
        [Fact]
        public void Serialise_BasicObject_ReturnsCorrectJson()
        {
            // Arrange
            var obj = new { a = 1 };
            var expected = Encoding.UTF8.GetBytes("{\"a\":1}");

            // Act
            var output = SerialisationUtil.Serialise(obj);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void Deserialise_BasicString_ReturnsString()
        {
            // Arrange
            var input = Encoding.UTF8.GetBytes("'string'");
            var expected = "string";

            // Act
            var output = SerialisationUtil.Deserialise<string>(input);

            // Assert
            Assert.Equal(expected, output);
        }
    }
}