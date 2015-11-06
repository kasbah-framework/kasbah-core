using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Index.Attributes;

namespace Kasbah.Core.Index.Utils
{
    /// <summary>
    /// Used for serialising objects for the index
    /// </summary>
    public static class SerialisationUtil
    {
        public static IDictionary<string, object> Serialise(object input)
        {
            if (input == null) { throw new ArgumentNullException(nameof(input)); }

            var type = input.GetType();
            var properties = type.GetProperties()
                .Where(ent => !ent.GetCustomAttributes(typeof(IndexIgnoreAttribute), true).Any());

            return properties.ToDictionary(ent => ent.Name, ent => ent.GetValue(input, null));
        }
    }
}