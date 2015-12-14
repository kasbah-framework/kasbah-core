using System.Collections.Generic;
using Xunit;

namespace Kasbah.Core.Events.Tests
{
    public class ModelTests
    {
        #region Public Methods

        [Fact]
        public void EventBase_GetAndSetData_DoesntLoseValue()
        {
            // Arrange
            var @event = new TestEvent();

            // Act
            @event.Payload = "hello";


            // Assert
            Assert.Equal("hello", @event.Payload);
        }

        #endregion

        #region Internal Classes

        internal class TestEvent : EventBase<string>
        {
        }

        #endregion
    }
}