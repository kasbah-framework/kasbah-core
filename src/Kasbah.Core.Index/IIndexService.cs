using System.Collections.Generic;

namespace Kasbah.Core.Index
{
    public interface IIndexService
    {
        void Register<T>(IIndexHandler handler);

        void Unregister(IIndexHandler handler);

        void Unregister<T>(IIndexHandler handler);

        IEnumerable<IDictionary<string, object>> Query(string query);
    }
}