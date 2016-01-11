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

            var dict = item.GetType().GetAllProperties()
                // .Where(ent => ent.GetAttributeValue<SystemFieldAttribute, bool>(a => a == null))
                .ToDictionary(ent => nameResolver.GetResolvedPropertyName(ent.Name),
                    ent => ent.GetValue(item, null));

            // TODO: type mapping -- unit test this and make it better
            foreach (var key in dict.Keys)
            {
                var value = dict[key];
                if (typeof(ItemBase).IsAssignableFrom(value.GetType()))
                {
                    dict[key] = (value as ItemBase).Id;
                }
                else if (typeof(IEnumerable<ItemBase>).IsAssignableFrom(value.GetType()))
                {
                    dict[key] = (value as IEnumerable<ItemBase>).Select(ent => ent.Id);
                }
            }

            return dict;
        }

        #endregion
    }
}