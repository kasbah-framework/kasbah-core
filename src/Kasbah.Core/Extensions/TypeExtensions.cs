using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kasbah.Core
{
    public static class TypeExtensions
    {
        #region Public Methods

        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            return type.GetRuntimeProperties();
        }

        public static TRet GetAttributeValue<TAttr, TRet>(this MemberInfo info, Func<TAttr, TRet> selector)
            where TAttr : Attribute
        {
            var attribute = info.GetCustomAttribute<TAttr>();

            return selector(attribute);
        }

        #endregion
    }
}