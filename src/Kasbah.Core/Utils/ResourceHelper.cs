using System;
using System.IO;

namespace Kasbah.Core.Utils
{
    public static class ResourceUtil
    {
        #region Public Methods

        public static string Get<TAssembly>(string name)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }

            name = name.Replace('/', '.');
            name = typeof(TAssembly).Assembly.GetName().Name + '.' + name;

            using (var stream = typeof(TAssembly).Assembly.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion
    }
}