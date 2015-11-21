using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var typeInfo = input.GetType().GetTypeInfo();
            if (!_propertyCache.ContainsKey(typeInfo))
            {
                // Get only the properties that aren't ignored
                _propertyCache[typeInfo] = typeInfo.DeclaredProperties
                    .Where(ent => !ent.CustomAttributes
                        .Select(attr => attr.AttributeType)
                        .Contains(typeof(IndexIgnoreAttribute)));
            }

            return _propertyCache[typeInfo].ToDictionary(ent => ent.Name, ent => ent.GetValue(input, null));
        }

        static IDictionary<TypeInfo, IEnumerable<PropertyInfo>> _propertyCache = new Dictionary<TypeInfo, IEnumerable<PropertyInfo>>();
    }
}