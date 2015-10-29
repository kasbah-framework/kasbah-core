using Xunit;
using Kasbah.Core.Events;
using System;
using System.Collections.Generic;

namespace Kasbah.Core.Events.Tests
{
    public class EventServiceTests
    {
        //  [Fact]
        // I need to do a bit of research on current best practices with naming methods
        // before we continue down this path - Chris
        //  public void Emit_TypeSetDataNull_CorrectTypeAndNullDataIsPassed()
        //  {
        //      var bus = new EventService(); // TODO: Put in test harness

        //      string expectedType = "test.events";

        //      string actualType = "";
        //      object actualData = null;

        //      bus.RegisterListener("test.events", (string type, object data) => {
        //          actualType = type;
        //          actualData = data;
        //      });

        //      bus.Emit("test.events");

        //      Assert.True(actualType.Equals(expectedType));
        //      Assert.True(actualData == null);
        //  }

        // Test multiple handlers
        // Test data pass back
        // Add a remove method to the event service

        [Fact]
        public void Emit_SingleEvent_EventHandled()
        {
            // Arrange
            var service = new EventService();
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
            var service = new EventService();
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
        public void Emit_HandlerNotRegistered_WontReceiveEvents()
        {
            // Arrange
            var service = new EventService();
            var handler = new EventHandler();

            // Act
            service.Emit(new TestEvent());

            // Assert
            Assert.Empty(handler.HandledEvents);
        }
    }

    internal class EventHandler : IEventHandler
    {
        public ICollection<EventBase> HandledEvents = new List<EventBase>();

        public void HandleEvent<T>(T @event) where T : EventBase
        {
            HandledEvents.Add(@event);
        }
    }

    internal class TestEvent : EventBase
    {

    }
}