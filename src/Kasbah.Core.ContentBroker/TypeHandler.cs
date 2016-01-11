using System;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Annotations;
using Kasbah.Core.Models;
using Kasbah.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.ContentBroker
{
    public class TypeHandler
    {
        #region Public Constructors

        public TypeHandler()
        {
        }

        #endregion

        #region Public Methods

        // TODO: passing in these functions is gross and needs to be replaced.
        public object MapDictToItem(IDictionary<string, object> dict, Type type, Func<Guid, Node> GetNode, Func<Guid, Guid, Type, object> GetNodeVersion)
        {
            var item = Activator.CreateInstance(type);
            var nameResolver = new CamelCasePropertyNamesContractResolver();
            var properties = type.GetAllProperties()
                .Where(ent => ent.GetAttributeValue<SystemFieldAttribute, bool>(a => a == null));

            foreach (var prop in properties)
            {
                var name = nameResolver.GetResolvedPropertyName(prop.Name);
                object val;
                if (dict.TryGetValue(name, out val))
                {
                    // TODO: type mapping --unit test this and make it better
                    if (typeof(ItemBase).IsAssignableFrom(prop.PropertyType) && val != null)
                    {
                        var reference = Guid.Parse(val.ToString());

                        var refNode = GetNode(reference);

                        if (refNode.ActiveVersion.HasValue)
                        {
                            val = GetNodeVersion(refNode.Id, refNode.ActiveVersion.Value, TypeUtil.TypeFromName(refNode.Type));
                        }
                        else
                        {
                            val = null;
                        }
                    }
                    else if (typeof(IEnumerable<ItemBase>).IsAssignableFrom(prop.PropertyType) && val != null)
                    {
                        var reference = JsonConvert.DeserializeObject<IEnumerable<Guid>>(val.ToString());

                        var refNodes = reference.Select(GetNode);

                        val = refNodes.Where(ent => ent.ActiveVersion.HasValue).Select(ent => GetNodeVersion(ent.Id, ent.ActiveVersion.Value, TypeUtil.TypeFromName(ent.Type)));
                    }

                    prop.SetValue(item, val, null);
                }
            }

            if (dict.ContainsKey("id"))
            {
                var id = Guid.Parse(dict["id"] as string);

                // TODO: this needs to be done better.
                var nodeProperty = type.GetProperty("Node");
                nodeProperty?.SetValue(item, GetNode(id), null);
            }

            return item;
        }

        public IDictionary<string, object> MapItemToDict(object item)
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