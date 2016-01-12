using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kasbah.Core
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
#if DNXCORE50
            if (type == null) { return Enumerable.Empty<PropertyInfo>(); }

            var info = type.GetTypeInfo();

            return info.DeclaredProperties.Concat(GetAllProperties(info.BaseType));
#else
            return type.GetProperties(BindingFlags.Public);
#endif
        }

        public static TRet GetAttributeValue<TAttr, TRet>(this MemberInfo info, Func<TAttr, TRet> selector)
            where TAttr : Attribute
        {
#if DNXCORE50

            var attr = info.CustomAttributes.SingleOrDefault(ent => ent.AttributeType == typeof(TAttr));
            if (attr != null)
            {
                return selector(attr as TAttr);
            }

            return default(TRet);
#else
            var attribute = info.GetCustomAttribute<TAttr>();

            return selector(attribute);
#endif
        }
    }
}