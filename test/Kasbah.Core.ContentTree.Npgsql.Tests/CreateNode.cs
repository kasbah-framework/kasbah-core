using System;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Moq;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class Npgsql_CreateNode
    {
        #region Public Methods

        [DbFact]
        public void CreateNode_WithCorrectProperties_NoExceptionThrown()
        {
            // Arrange
            var eventService = Mock.Of<IEventService>();
            var service = new NpgsqlContentTreeProvider();

            // Act
            service.CreateNode(Guid.NewGuid(), null, Guid.NewGuid().ToString(), typeof(EmptyItem).AssemblyQualifiedName);

            // Assert
            // -- no exception = Pass
        }

        #endregion
    }
}