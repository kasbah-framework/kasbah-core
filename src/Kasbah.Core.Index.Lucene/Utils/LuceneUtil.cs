using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.Index.Lucene
{
    public static class LuceneUtil
    {
        #region Public Methods

        public static IDictionary<string, object> ConverFromSolr(IDictionary<string, object> input)
        {
            return input
                .Select(ProcessValueFromSolr)
                .ToDictionary(ent => ent.Key, ent => ent.Value);
        }

        public static IDictionary<string, object> ConvertToSolr(IDictionary<string, object> input)
        {
            return input
                .Select(ProcessValueToSolr)
                .ToDictionary(ent => ent.Key, ent => ent.Value);
        }

        #endregion

        #region Private Fields

        static CamelCasePropertyNamesContractResolver _nameResolver = new CamelCasePropertyNamesContractResolver();

        #endregion

        #region Private Methods

        static KeyValuePair<string, object> ProcessValueFromSolr(KeyValuePair<string, object> kvp)
        {
            var key = kvp.Key;
            var value = kvp.Value;

            if (key != "id")
            {
                if (key.EndsWith(TypeSuffixes.Int))
                {
                    key = key.Substring(0, key.Length - TypeSuffixes.Int.Length);
                    value = Convert.ToInt32(value);
                }
                else if (key.EndsWith(TypeSuffixes.String))
                {
                    key = key.Substring(0, key.Length - TypeSuffixes.String.Length);
                    value = Convert.ToString(value);
                }
                else if (key.EndsWith(TypeSuffixes.DateTime))
                {
                    key = key.Substring(0, key.Length - TypeSuffixes.DateTime.Length);
                    value = Convert.ToDateTime(value);
                }
                else if (key.EndsWith(TypeSuffixes.Double))
                {
                    key = key.Substring(0, key.Length - TypeSuffixes.Double.Length);
                    value = Convert.ToDouble(value);
                }
                else if (key.EndsWith(TypeSuffixes.Bool))
                {
                    key = key.Substring(0, key.Length - TypeSuffixes.Bool.Length);
                    value = Convert.ToBoolean(value);
                }
                else if (key.EndsWith(TypeSuffixes.Guid))
                {
                    key = key.Substring(0, key.Length - TypeSuffixes.Guid.Length);
                    value = Guid.Parse(value as string);
                }
                else if (key.EndsWith(TypeSuffixes.General))
                {
                    key = key.Substring(0, key.Length - TypeSuffixes.General.Length);
                }
            }

            return new KeyValuePair<string, object>(key, value);
        }

        static KeyValuePair<string, object> ProcessValueToSolr(KeyValuePair<string, object> kvp)
        {
            var key = kvp.Key;
            var value = kvp.Value;

            if (key != "id")
            {
                if (value is int)
                {
                    key = key + TypeSuffixes.Int;
                }
                else if (value is string)
                {
                    key = key + TypeSuffixes.String;
                }
                else if (value is DateTime)
                {
                    key = key + TypeSuffixes.DateTime;
                    value = $"{value:o}";
                }
                else if (value is double)
                {
                    key = key + TypeSuffixes.Double;
                }
                else if (value is bool)
                {
                    key = key + TypeSuffixes.Bool;
                }
                else if (value is Guid)
                {
                    key = key + TypeSuffixes.Guid;
                    value = value.ToString();
                }
                else
                {
                    key = key + TypeSuffixes.General;
                }
            }

            return new KeyValuePair<string, object>(key, value);
        }

        #endregion

        #region Private Classes

        static class TypeSuffixes
        {
            #region Public Fields

            public const string Bool = "_b";
            public const string DateTime = "_dt";
            public const string Double = "_d";
            public const string General = "_s";
            public const string Guid = "_guid_s";
            public const string Int = "_i";
            public const string String = "_t";

            #endregion
        }

        #endregion
    }
}