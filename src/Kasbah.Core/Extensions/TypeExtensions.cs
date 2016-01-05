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
            if (type == null) { return Enumerable.Empty<PropertyInfo>(); }

            var info = type.GetTypeInfo();

            return info.DeclaredProperties.Concat(GetAllProperties(info.BaseType));
        }

        public static TRet GetAttributeValue<TAttr, TRet>(this MemberInfo info, Func<TAttr, TRet> selector)
            where TAttr : Attribute
        {
            var attribute = info.GetCustomAttribute<TAttr>();

            return selector(attribute);
        }
    }
}