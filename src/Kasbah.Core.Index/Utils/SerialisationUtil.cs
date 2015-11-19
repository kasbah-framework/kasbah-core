using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Index.Attributes;
using System.Reflection;

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

            var typeInfo = input.GetType().GetTypeInfo();
            var properties = typeInfo.DeclaredProperties
                .Where(ent => !ent.CustomAttributes.Any(attr => attr.GetType() == typeof(IndexIgnoreAttribute)));

            return properties.ToDictionary(ent => ent.Name, ent => ent.GetValue(input, null));
        }
    }
}