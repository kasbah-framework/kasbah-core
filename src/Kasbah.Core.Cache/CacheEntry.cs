using System.Collections.Generic;

namespace Kasbah.Core.Cache
{
    public class CacheEntry
    {
        public IEnumerable<string> Dependants { get; set; }

        public object Value { get; set; }
    }
}