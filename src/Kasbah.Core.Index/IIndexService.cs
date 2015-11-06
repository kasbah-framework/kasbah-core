using System.Collections.Generic;
using Kasbah.Core.Index.Models;

namespace Kasbah.Core.Index
{
    public interface IIndexService
    {
        void Register<T>(IIndexHandler handler);

        void Unregister(IIndexHandler handler);

        void Unregister<T>(IIndexHandler handler);

        IEnumerable<IndexItem> Query(string query);
    }
}