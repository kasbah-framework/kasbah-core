using Kasbah.Core.Utils;
using Xunit;

namespace Kasbah.Core.Tests
{
    public class TypeUtilTests
    {
        #region Public Methods

        [Fact]
        public void TypeName_ForExampleType_ReturnsCorrectName()
        {
            // Arrange
            const string Expected = "Kasbah.Core.Tests.ExampleType, Kasbah.Core.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            // Act
            var typeName = TypeUtil.TypeName<ExampleType>();

            // Assert
            Assert.Equal(Expected, typeName);
        }

        [Fact]
        public void TypeFromName_ForExampleType_ReturnsExampleType()
        {
            // Arrange
            const string TypeName = "Kasbah.Core.Tests.ExampleType, Kasbah.Core.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            // Act
            var type = TypeUtil.TypeFromName(TypeName);

            // Assert
            Assert.Equal(typeof(ExampleType), type);
        }

        [Fact]
        public void TypeFromName_AfterRegisteringType_ReturnsCorrectType()
        {
            // Arrange
            TypeUtil.Register<ExampleType>();

            // Act
            var type = TypeUtil.TypeFromName(typeof(ExampleType).AssemblyQualifiedName);

            // Assert
            Assert.NotNull(type);
            Assert.Equal(type.AssemblyQualifiedName, typeof(ExampleType).AssemblyQualifiedName);
        }

        #endregion
    }

    internal class ExampleType { }
}