using System;

namespace Kasbah.Core.Utils
{
    public static class TypeUtil
    {
        #region Public Methods

        public static Type TypeFromName(string name)
        {
            return Type.GetType(name);
        }

        public static string TypeName<T>()
        {
            return typeof(T).AssemblyQualifiedName;
        }

        #endregion
    }
}