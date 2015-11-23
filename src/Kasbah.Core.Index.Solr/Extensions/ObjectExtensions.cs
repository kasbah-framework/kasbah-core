using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index.Solr
{
    public static class ObjectExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : ItemBase, new()
        {
            var ret = new T();
            var typeInfo = typeof(T).GetTypeInfo();
            
            foreach (var item in source)
            {
                typeInfo.DeclaredProperties
                    .SingleOrDefault(e => e.Name == item.Key)?
                    .SetValue(ret, item.Value, null);
            }

            return ret;
        }

        public static IDictionary<string, object> AsDictionary(this object source)
        {
            return source.GetType().GetTypeInfo().DeclaredProperties.ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }
    }
}