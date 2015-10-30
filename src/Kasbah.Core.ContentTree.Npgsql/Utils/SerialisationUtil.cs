using System;
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

        public static object Deserialise(string input, Type type)
        {
            return JsonConvert.DeserializeObject(input, type, _settings);
        }

        public static T Deserialise<T>(string input)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(input, _settings);
        }

        public static string Serialise(object input)
        {
            return JsonConvert.SerializeObject(input, _settings);
        }

        #endregion
    }
}