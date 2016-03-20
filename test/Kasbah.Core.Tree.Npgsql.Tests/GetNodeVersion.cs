namespace Kasbah.Core.Tree.Npgsql.Tests
{
    public class Npgsql_GetNodeVersion
    {
        #region Public Methods

        //[DbFact]
        //public void GetNodeVersion_WhereVersionExists_ReturnsCorrectVersion()
        //{
        //    // Arrange
        //    var service = new NpgsqlTreeProvider(Mock.Of<ILoggerFactory>());

        //    var item = new TestItem { Value = Guid.NewGuid().ToString() };

        //    var id = Guid.NewGuid();
        //    service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(TestItem).FullName);

        //    var version = service.Save<TestItem>(Guid.NewGuid(), id, item);

        //    // Act
        //    var outItem = service.GetNodeVersion(id, version.Id, typeof(TestItem)) as TestItem;

        //    // Assert
        //    Assert.NotNull(outItem);
        //    Assert.Equal(item.Value, outItem.Value);
        //}

        //[DbFact]
        //public void GetNodeVersion_WhereVersionDoesNotExist_ReturnsNull()
        //{
        //    // Arrange
        //    var service = new NpgsqlTreeProvider(Mock.Of<ILoggerFactory>());

        //    var id = Guid.NewGuid();
        //    service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(TestItem).FullName);

        //    // Act
        //    var outItem = service.GetNodeVersion(id, Guid.Empty, typeof(TestItem)) as TestItem;

        //    // Assert
        //    Assert.Null(outItem);
        //}

        //[DbFact]
        //public void GetNodeVersion_WithoutType_ReturnsValidDictionary()
        //{
        //    // Arrange
        //    var service = new NpgsqlTreeProvider(Mock.Of<ILoggerFactory>());

        //    var item = new TestItem { Value = Guid.NewGuid().ToString() };

        //    var id = Guid.NewGuid();
        //    service.CreateNode(id, null, Guid.NewGuid().ToString(), typeof(TestItem).FullName);
        //    var version = service.Save<TestItem>(Guid.NewGuid(), id, item);

        //    // Act
        //    var outItem = service.GetNodeVersion(id, version.Id);

        //    // Assert
        //    Assert.NotNull(outItem);
        //    Assert.True(outItem.ContainsKey("value"));
        //    Assert.Equal(item.Value, outItem["value"]);
        //}

        #endregion
    }
}