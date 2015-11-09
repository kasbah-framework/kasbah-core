using System;
using System.Collections.Generic;
using System.Linq;

namespace Kasbah.Core.Utils
{
    public static class TypeUtil
    {
        #region Public Methods

        public static Type TypeFromName(string name)
        {
            return _types.SingleOrDefault(ent => ent.AssemblyQualifiedName == name)
                ?? Type.GetType(name);
        }

        public static string TypeName<T>()
        {
            return typeof(T).AssemblyQualifiedName;
        }

        public static void Register<T>()
        {
            _types.Add(typeof(T));
        }

        static ICollection<Type> _types = new List<Type>();

        #endregion
    }
}