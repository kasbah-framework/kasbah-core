using System;
using System.Collections.Generic;
using System.Linq;

namespace Kasbah.Core.Events
{
        public class EventService : IEventService 
        {
                public List<KeyValuePair<string,IList<KasbahEventHandler>>> _listeners { get; set; }
                
                public EventService()
                {
                        _listeners = new List<KeyValuePair<string,IList<KasbahEventHandler>>>();
                }
                
                public void RegisterListener(string type, KasbahEventHandler handler) 
                {
                        var key = _listeners.FirstOrDefault(u => u.Key.Equals(type));
                        
                        if (key.Equals(default(KeyValuePair<string, IList<KasbahEventHandler>>)))
                        {
                                key = new KeyValuePair<string,IList<KasbahEventHandler>>(type, new List<KasbahEventHandler>());
                                _listeners.Add(key);
                        }
                        
                        key.Value.Add(handler);
                }
                
                // Generics to strongly type this someway?
                // Try Catch to handle any error scenarios
                
                public void Emit(string type)
                {
                        Emit(type, null);
                }
                
                public void Emit(string type, object data) 
                {
                        var key = _listeners.FirstOrDefault(u => u.Key.Equals(type));
                        
                        foreach(KasbahEventHandler listener in key.Value) 
                        {
                                listener(type, data);
                        }
                }
        }
}