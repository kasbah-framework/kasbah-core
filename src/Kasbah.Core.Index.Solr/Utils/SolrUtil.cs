using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.Index.Solr
{
    public static class SolrUtil
    {
        static CamelCasePropertyNamesContractResolver _nameResolver = new CamelCasePropertyNamesContractResolver();

        public static IDictionary<string, object> ConvertToSolr(IDictionary<string, object> input)
        {
            return input
                .Select(ProcessValue)
                .ToDictionary(ent => ent.Key, ent => ent.Value);
        }

        static KeyValuePair<string, object> ProcessValue(KeyValuePair<string, object> kvp)
        {
            var key = kvp.Key;
            var value = kvp.Value;

            if (key != "id")
            {
                key = _nameResolver.GetResolvedPropertyName(key);

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
    }
}