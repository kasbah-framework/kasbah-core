using System;
using Xunit;

namespace Kasbah.Core.Tests
{
    public class ResourceUtilTests
    {
        [Fact]
        public void Get_ExistingResource_ReturnsContents()
        {
            // Arrange
            const string ResourceName = "Resources.ExampleResource.txt";

            // Act
            var contents = Utils.ResourceUtil.Get<ResourceUtilTests>(ResourceName);

            // Assert
            Assert.Equal("Hello.", contents);
        }

        [Fact]
        public void Get_NonExistentResource_ThrowsException()
        {
            // Arrange
            const string ResourceName = "Resources.__NON_EXISTENT__.txt";

            // Assert
            Assert.Throws<Utils.ResourceNotFoundException>(() =>
            {
                Utils.ResourceUtil.Get<ResourceUtilTests>(ResourceName);
            });
        }

        [Fact]
        public void Get_NullName_ThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                Utils.ResourceUtil.Get<ResourceUtilTests>(null);
            });
        }
    }
}