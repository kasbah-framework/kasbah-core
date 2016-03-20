using System;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kasbah.Core.Tree.Npgsql.Tests
{
    public class Npgsql_CreateNode
    {
        #region Public Methods

        [DbFact]
        public void CreateNode_WithCorrectProperties_NoExceptionThrown()
        {
            // Arrange
            var service = new NpgsqlTreeProvider(Mock.Of<ILoggerFactory>());

            // Act
            service.CreateNode(Guid.NewGuid(), null, Guid.NewGuid().ToString(), typeof(EmptyItem).AssemblyQualifiedName);

            // Assert
            // -- no exception = Pass
        }

        #endregion
    }

    class EmptyItem
    {

    }
}