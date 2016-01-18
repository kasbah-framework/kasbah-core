using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kasbah.Core.Models;

namespace Kasbah.Core.Index.Solr
{
    public static class ObjectExtensions
    {
        #region Public Methods

        public static IDictionary<string, object> AsDictionary(this object source)
        {
            return source.GetType().GetTypeInfo().DeclaredProperties.ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }
        
        #endregion
    }
}