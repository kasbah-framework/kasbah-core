using Xunit;

namespace Kasbah.Core.Cache.Tests
{
    public class Dependencies
    {
        [Fact]
        public void InvalidateItem_WithDependencies_InvalitesDependencies()
        {
            // Arrange
            var provider = new InMemoryCache();

            var service = new CacheService(provider);

            var depKey = "dep_A";
            var origDep = new object();
            service.GetOrSet(depKey, typeof(object), () => origDep);

            service.GetOrSet("test", typeof(object),
                () => new object(),
                (obj) => new[] { depKey });

            // Act
            service.Remove("test");

            // Assert
            var cachedDep = service.GetOrSet(depKey, typeof(object), () => null);

            Assert.Null(cachedDep);
        }
        [Fact]
        public void InvalidateItem_WithDependencies_RecursivelyInvalitesDependencies()
        {
            // Arrange
            var provider = new InMemoryCache();

            var service = new CacheService(provider);

            var depAKey = "dep_A";
            service.GetOrSet(depAKey, typeof(object), () => new object());

            var depBKey = "dep_B";
            service.GetOrSet(depBKey, typeof(object),
                () => new object(),
                (obj) => new[] { depAKey });


            service.GetOrSet("test", typeof(object),
                () => new object(),
                (obj) => new[] { depBKey });

            // Act
            service.Remove("test");

            // Assert
            var cachedDepA = service.GetOrSet(depAKey, typeof(object), () => null);
            var cachedDepB = service.GetOrSet(depBKey, typeof(object), () => null);

            Assert.Null(cachedDepA);
            Assert.Null(cachedDepB);
        }
    }
}