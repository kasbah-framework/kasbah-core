using System;
using System.IO;
using System.Reflection;

namespace Kasbah.Core.Utils
{
    public static class ResourceUtil
    {
        #region Public Methods

        public static string Get<TAssembly>(string name)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }

            var assembly = typeof(TAssembly).GetTypeInfo().Assembly;

            name = name.Replace('/', '.');
            name = assembly.GetName().Name + '.' + name;

            using (var stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                {
                    throw new ResourceNotFoundException(name);
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }

    public class ResourceNotFoundException : Exception
    {
        #region Public Constructors

        public ResourceNotFoundException(string resourceName)
            : base($"Resource not found {resourceName}")
        {
        }

        #endregion
    }
}