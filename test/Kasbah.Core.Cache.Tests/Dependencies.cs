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
            var service = new CacheService(Mock.Of<IDistributedCache>());

            service.GetOrSet("test", typeof(object),
                () => new object(),
                (obj) => null);

            // Act
            service.Remove("test");

            // Assert
        }
    }
}