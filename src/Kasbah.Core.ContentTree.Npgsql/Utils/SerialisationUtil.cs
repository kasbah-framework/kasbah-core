using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.ContentTree.Npgsql.Utils
{
    public static class SerialisationUtil
    {
        #region Private Fields

        static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

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