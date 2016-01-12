using System.Linq;
using Kasbah.Core.Annotations;
using Xunit;

namespace Kasbah.Core.Tests
{
    public class TypeExtensionTests
    {
        #region Public Methods

        [Fact]
        public void GetAllProperties_ReturnsAllProperties()
        {
            // Arrange
            var type = typeof(ExampleType);

            // Act
            var properties = TypeExtensions.GetAllProperties(type);

            // Assert
            Assert.NotEmpty(properties);
            Assert.Contains("Prop", properties.Select(ent => ent.Name));
        }

        [Fact]
        public void GetAttributeValue_WithExistingAttribute_ReturnsExpectedValue()
        {
            // Arrange
            var type = typeof(ExampleType);

            // Act
            var result = TypeExtensions.GetAttributeValue<SystemFieldAttribute, bool>(type.GetAllProperties().Single(ent => ent.Name == "SystemField"), (_) => { return true; });

            // Assert
            Assert.True(result);
        }

        #endregion
    }
}