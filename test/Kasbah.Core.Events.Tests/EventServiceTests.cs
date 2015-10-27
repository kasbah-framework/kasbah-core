using Xunit;
using Kasbah.Core.Events;

namespace Kasbah.Core.Events.Tests
{
    public class EventServiceTests
    {
        [Fact]
        // I need to do a bit of research on current best practices with naming methods
        // before we continue down this path - Chris
        public void Emit_TypeSetDataNull_CorrectTypeAndNullDataIsPassed()
        {
            var bus = new EventService(); // TODO: Put in test harness
            
            string expectedType = "test.events";
            
            string actualType = "";
            object actualData = null;
            
            bus.RegisterListener("test.events", (string type, object data) => {
                actualType = type;
                actualData = data;
            });
            
            bus.Emit("test.events");
            
            Assert.True(actualType.Equals(expectedType));
            Assert.True(actualData == null);
        }
        
        // Test multiple handlers
        // Test data pass back
        // Add a remove method to the event service
        
    }
}