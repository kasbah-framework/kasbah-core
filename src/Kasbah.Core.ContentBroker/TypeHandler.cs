using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.ContentBroker
{
    public static class TypeHandler
    {
        #region Public Methods

        public static object MapDictToItem(IDictionary<string, object> dict, Type type, ContentBroker contentBroker)
        {
            return new ItemBaseProxy(type, dict, contentBroker).GetTransparentProxy();
        }

        public static IDictionary<string, object> MapItemToDict(object item)
        {
            if (item == null) { return null; }

            if (item is IDictionary<string, object>) { return item as IDictionary<string, object>; }
            if (item is JObject) { return (item as JObject).ToObject<IDictionary<string, object>>(); }

            var nameResolver = new CamelCasePropertyNamesContractResolver();

            var props = item.GetType().GetAllProperties();
            var dict = new Dictionary<string, object>();

            foreach (var prop in props)
            {
                var name = nameResolver.GetResolvedPropertyName(prop.Name);
                var value = prop.GetValue(item, null);

                if (value == null)
                {
                    dict[name] = null;
                }
                else if (typeof(ItemBase).IsAssignableFrom(value.GetType()))
                {
                    dict[name] = (value as ItemBase).Id;
                }
                else if (typeof(IEnumerable<ItemBase>).IsAssignableFrom(value.GetType()))
                {
                    dict[name] = (value as IEnumerable<ItemBase>).Select(ent => ent.Id);
                }
                else
                {
                    dict[name] = value;
                }
            }
            return dict;
        }

        #endregion
    }
}