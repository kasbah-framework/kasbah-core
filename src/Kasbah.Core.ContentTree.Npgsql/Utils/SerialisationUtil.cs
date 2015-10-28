using System;
using Newtonsoft.Json;
using System.Text;

namespace Kasbah.Core.ContentTree.Npgsql.Utils
{
    public static class SerialisationUtil
    {
        public static byte[] Serialise(object input)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(input));
        }

        public static object Deserialise(byte[] input, Type type)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(input), type);
        }

        public static T Deserialise<T>(byte[] input)
            where T : class
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(input));
        }
    }
}