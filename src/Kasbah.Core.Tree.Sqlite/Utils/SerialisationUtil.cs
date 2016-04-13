using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kasbah.Core.Tree.Sqlite.Utils
{
    public static class SerialisationUtil
    {
        #region Private Fields

        static JsonSerializerSettings _settings = new JsonSerializerSettings { };

        #endregion

        #region Public Methods

        public static IDictionary<string, object> Deserialise(string input)
        {
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(input, _settings);
        }

        public static string Serialise(object input)
        {
            return JsonConvert.SerializeObject(input, _settings);
        }

        #endregion
    }

}