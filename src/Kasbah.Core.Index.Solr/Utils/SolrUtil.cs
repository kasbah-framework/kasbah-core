using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.Index.Solr
{
    public static class SolrUtil
    {
        static CamelCasePropertyNamesContractResolver _nameResolver = new CamelCasePropertyNamesContractResolver();

        public static IDictionary<string, object> ConverFromSolr(IDictionary<string, object> input)
        {
            // TODO: fix this
            return input
                .Select(ProcessValueFromSolr)
                .ToLookup(ent => ent.Key, ent => ent.Value)
                .ToDictionary(ent => ent.Key, ent => ent.First());
        }

        public static IDictionary<string, object> ConvertToSolr(IDictionary<string, object> input)
        {
            return input
                .Select(ProcessValueToSolr)
                .ToDictionary(ent => ent.Key, ent => ent.Value);
        }

        static KeyValuePair<string, object> ProcessValueToSolr(KeyValuePair<string, object> kvp)
        {
            var key = kvp.Key;
            var value = kvp.Value;

            if (key != "id")
            {
                if (value is int)
                {
                    key = key + "_i";
                }
                else if (value is string)
                {
                    key = key + "_t";
                }
                else if (value is DateTime)
                {
                    key = key + "_dt";
                    value = $"{value:o}";
                }
                else if (value is double)
                {
                    key = key + "_d";
                }
                else if (value is bool)
                {
                    key = key + "_b";
                }
                else
                {
                    key = key + "_s";
                }
            }

            return new KeyValuePair<string, object>(key, value);
        }

        static KeyValuePair<string, object> ProcessValueFromSolr(KeyValuePair<string, object> kvp)
        {
            var key = kvp.Key;
            var value = kvp.Value;

            if (key != "id")
            {
                if (key.EndsWith("_i"))
                {
                    key = key.Substring(key.Length - 2);
                    value = Convert.ToInt32(value);
                }
                else if (key.EndsWith("_t"))
                {
                    key = key.Substring(key.Length - 2);
                    value = Convert.ToString(value);
                }
                else if (key.EndsWith("_dt"))
                {
                    key = key.Substring(key.Length - 3);
                    value = Convert.ToDateTime(value);
                }
                else if (key.EndsWith("_d"))
                {
                    key = key.Substring(key.Length - 2);
                    value = Convert.ToDouble(value);
                }
                else if (key.EndsWith("_b"))
                {
                    key = key.Substring(key.Length - 2);
                    value = Convert.ToBoolean(value);
                }
                else if (key.EndsWith("_s"))
                {
                    key = key.Substring(key.Length - 2);
                }
            }

            return new KeyValuePair<string, object>(key, value);
        }
    }
}