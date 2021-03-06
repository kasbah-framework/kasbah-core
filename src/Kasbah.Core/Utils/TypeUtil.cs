﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Kasbah.Core.Utils
{
    public static class TypeUtil
    {
        #region Public Methods

        public static void Register<T>()
        {
            Register(typeof(T));
        }

        public static void Register(Type type)
        {
            if (!_types.Contains(type))
            {
                _types.Add(type);
            }
        }

        public static Type TypeFromName(string name)
        {
            if (name == null) { return null; }

            return _types.SingleOrDefault(ent => ent.AssemblyQualifiedName == name)
                ?? Type.GetType(name);
        }

        public static string TypeName<T>()
        {
            return typeof(T).AssemblyQualifiedName;
        }

        static ICollection<Type> _types = new List<Type>();

        #endregion
    }
}