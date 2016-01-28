using System.Collections.Generic;

namespace Kasbah.Core.Cache
{
    public class CacheEntry : CacheEntryBase
    {
        #region Public Properties

        public object Value { get; set; }

        #endregion
    }

    public class CacheEntry<T> : CacheEntryBase
    {
        #region Public Properties

        public T Value { get; set; }

        #endregion
    }

    public abstract class CacheEntryBase
    {
        #region Public Properties

        public IEnumerable<string> Dependencies { get; set; }

        #endregion
    }
}