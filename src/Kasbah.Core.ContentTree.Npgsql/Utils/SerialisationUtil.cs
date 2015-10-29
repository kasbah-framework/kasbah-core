using System;
using Newtonsoft.Json;
using System.Text;

namespace Kasbah.Core.ContentTree.Npgsql.Utils
{
    public static class SerialisationUtil
    {
        public static string Serialise(object input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public static object Deserialise(string input, Type type)
        {
            return JsonConvert.DeserializeObject(input, type);
        }

        public static T Deserialise<T>(string input)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
    }
}