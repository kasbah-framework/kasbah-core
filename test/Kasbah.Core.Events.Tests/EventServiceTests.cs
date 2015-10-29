using System.Collections.Generic;
using Xunit;

namespace Kasbah.Core.Events.Tests
{
    public class EventServiceTests
    {
        #region Public Methods

        [Fact]
        public void Emit_HandlerNotRegistered_WontReceiveEvents()
        {
            // Arrange
            var service = new InProcEventService();
            var handler = new EventHandler();

            // Act
            service.Emit(new TestEvent());

            // Assert
            Assert.Empty(handler.HandledEvents);
        }

        [Fact]
        public void Emit_SingleEvent_EventHandled()
        {
            // Arrange
            var service = new InProcEventService();
            var handler = new EventHandler();
            var @event = new TestEvent();

            service.Register<TestEvent>(handler);

            // Act
            service.Emit(@event);

            // Assert
            Assert.Contains(@event, handler.HandledEvents);
        }

        [Fact]
        public void Emit_TwoEvents_EventsHandled()
        {
            // Arrange
            var service = new InProcEventService();
            var handler = new EventHandler();
            var @event1 = new TestEvent();
            var @event2 = new TestEvent();

            service.Register<TestEvent>(handler);

            // Act
            service.Emit(@event1);
            service.Emit(@event2);

            // Assert
            Assert.Contains(@event1, handler.HandledEvents);
            Assert.Contains(@event2, handler.HandledEvents);
        }

        [Fact]
        public void Emit_UnregisteredHandler_NoEventHandled()
        {
            // Arrange
            var service = new InProcEventService();
            var handler = new EventHandler();
            var @event = new TestEvent();

            service.Register<TestEvent>(handler);

            service.Unregister(handler);

            // Act
            service.Emit(@event);

            // Assert
            Assert.DoesNotContain(@event, handler.HandledEvents);
        }

        [Fact]
        public void Emit_UnregisteredHandlerAndType_NoEventHandled()
        {
            // Arrange
            var service = new InProcEventService();
            var handler = new EventHandler();
            var @event = new TestEvent();

            service.Register<TestEvent>(handler);

            service.Unregister<TestEvent>(handler);

            // Act
            service.Emit(@event);

            // Assert
            Assert.DoesNotContain(@event, handler.HandledEvents);
        }

        #endregion
    }

    internal class EventHandler : IEventHandler
    {
        #region Public Fields

        public ICollection<EventBase> HandledEvents = new List<EventBase>();

        #endregion

        #region Public Methods

        public void HandleEvent<T>(T @event) where T : EventBase
        {
            HandledEvents.Add(@event);
        }

        #endregion
    }

    internal class TestEvent : EventBase
    {
    }
}