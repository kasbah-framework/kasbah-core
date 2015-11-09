using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kasbah.Core.Events.Redis.Tests
{
    public class EventServiceTests
    {
        #region Public Methods

        [RedisFact]
        public void Emit_HandlerNotRegistered_WontReceiveEvents()
        {
            // Arrange
            var service = new RedisEventService();
            var handler = new EventHandler();

            // Act
            service.Emit(new TestEvent());

            // Assert
            Assert.Empty(handler.HandledEvents);
        }

        [RedisFact]
        public void Emit_SingleEvent_EventHandled()
        {
            // Arrange
            var service = new RedisEventService();
            var handler = new EventHandler();
            var @event = new TestEvent();

            service.Register<TestEvent>(handler);

            // Act
            service.Emit(@event);

            // Assert
            Assert.Contains(@event.Id, handler.HandledEvents.Cast<TestEvent>().Select(ent => ent.Id));
        }

        [RedisFact]
        public void Emit_TwoEvents_EventsHandled()
        {
            // Arrange
            var service = new RedisEventService();
            var handler = new EventHandler();
            var event1 = new TestEvent();
            var event2 = new TestEvent();

            service.Register<TestEvent>(handler);

            // Act
            service.Emit(event1);
            service.Emit(event2);

            // Assert
            Assert.Contains(@event1.Id, handler.HandledEvents.Cast<TestEvent>().Select(ent => ent.Id));
            Assert.Contains(@event2.Id, handler.HandledEvents.Cast<TestEvent>().Select(ent => ent.Id));
        }

        [RedisFact]
        public void Emit_TwoHandlers_BothHandlersCalled()
        {
            // Arrange
            var service = new RedisEventService();
            var handler1 = new EventHandler();
            var handler2 = new EventHandler();
            var @event = new TestEvent();

            service.Register<TestEvent>(handler1);
            service.Register<TestEvent>(handler2);

            // Act
            service.Emit(@event);

            // Assert
            Assert.Contains(@event.Id, handler1.HandledEvents.Cast<TestEvent>().Select(ent => ent.Id));
            Assert.Contains(@event.Id, handler2.HandledEvents.Cast<TestEvent>().Select(ent => ent.Id));
        }

        [RedisFact]
        public void Emit_UnregisteredHandler_NoEventHandled()
        {
            // Arrange
            var service = new RedisEventService();
            var handler = new EventHandler();
            var @event = new TestEvent();

            service.Register<TestEvent>(handler);

            service.Unregister(handler);

            // Act
            service.Emit(@event);

            // Assert
            Assert.DoesNotContain(@event.Id, handler.HandledEvents.Cast<TestEvent>().Select(ent => ent.Id));
        }

        [RedisFact]
        public void Emit_UnregisteredHandlerAndType_NoEventHandled()
        {
            // Arrange
            var service = new RedisEventService();
            var handler = new EventHandler();
            var @event = new TestEvent();

            service.Register<TestEvent>(handler);

            service.Unregister<TestEvent>(handler);

            // Act
            service.Emit(@event);

            // Assert
            Assert.DoesNotContain(@event.Id, handler.HandledEvents.Cast<TestEvent>().Select(ent => ent.Id));
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
            public Guid Id { get; set; } = Guid.NewGuid();
        }

        #endregion
    }
}