using System.Collections.Generic;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index
{
    public interface IIndexHandler
    {
        IDictionary<string, object> AddCustomFields(ItemBase item);

        int Priority { get; }
    }
}