using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace Kasbah.Core.Cache.Tests
{
    public class Dependencies
    {
        [Fact]
        public void InvalidateItem_WithDependencies_RecursivelyInvalitesDependencies()
        {
            // Arrange
            var provider = new Mock<IDistributedCache>();
            provider.Setup(e => e.Remove("dep")).Verifiable();

            var service = new CacheService(provider.Object);

            service.GetOrSet("test", typeof(object),
                () => new object(),
                (obj) => new[] { "dep" });

            // Act
            service.Remove("test");

            // Assert
            provider.VerifyAll();
        }
    }
}