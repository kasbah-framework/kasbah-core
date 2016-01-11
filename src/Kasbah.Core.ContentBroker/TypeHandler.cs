using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Kasbah.Core.Annotations;
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
            var proxiedItem = new ItemBaseProxy(type, dict, contentBroker).GetTransparentProxy();

            var nameResolver = new CamelCasePropertyNamesContractResolver();
            var properties = type.GetAllProperties()
                .Where(ent => ent.GetAttributeValue<SystemFieldAttribute, bool>(a => a == null));

            foreach (var prop in properties)
            {
                var name = nameResolver.GetResolvedPropertyName(prop.Name);
                object val;
                if (dict.TryGetValue(name, out val))
                {
                    prop.SetValue(proxiedItem, val, null);
                }
            }

            return proxiedItem;
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

    public class ItemBaseProxy : RealProxy
    {
        readonly object _instance;
        readonly ContentBroker _contentBroker;
        readonly IDictionary<string, object> _innerDict;

        public ItemBaseProxy(Type type, IDictionary<string, object> innerDict, ContentBroker contentBroker)
            : base(type)
        {
            _instance = Activator.CreateInstance(type);
            _innerDict = innerDict;
            _contentBroker = contentBroker;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;

            try
            {
                switch (method.Name)
                {
                    case "get_Node":
                    {
                        var id = Guid.Parse(_innerDict["id"] as string);

                        var result = _contentBroker.GetNode(id);

                        return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                    }
                    default:
                    {
                        var result = method.Invoke(_instance, methodCall.InArgs);

                        return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
                if (e is TargetInvocationException && e.InnerException != null)
                {
                    return new ReturnMessage(e.InnerException, msg as IMethodCallMessage);
                }

                return new ReturnMessage(e, msg as IMethodCallMessage);
            }
        }
    }
}