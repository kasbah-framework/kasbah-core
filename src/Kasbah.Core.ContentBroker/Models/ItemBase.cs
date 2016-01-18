using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kasbah.Core.Annotations;
using Kasbah.Core.Models;
using Kasbah.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.ContentBroker.Models
{
    public sealed class EmptyItem : ItemBase
    {
    }

#if DNXCORE50
    public abstract class ItemBase : DispatchProxy
#else

    public abstract class ItemBase : MarshalByRefObject
#endif
    {
        [SystemField]
        public Guid Id { get; set; }

        [SystemField]
        public Node Node { get; }

#if DNXCORE50
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var method = targetMethod;

            var ret = GetOrSetValue(method.Name, () =>
            {
                var result = default(object);
                var handled = false;
                if (method.Name.StartsWith("get_"))
                {
                    var name = NameResolver.GetResolvedPropertyName(method.Name.Substring("get_".Length));

                    switch (name)
                    {
                        case "node":
                            {
                                var id = Guid.Parse(_innerDict["id"] as string);

                                result = _contentBroker.GetNode(id);
                                handled = true;
                            }
                            break;

                        default:
                            if (_innerDict.ContainsKey(name))
                            {
                                result = _innerDict[name];

                                if (typeof(ItemBase).IsAssignableFrom(method.ReturnType))
                                {
                                    var refNodeId = Guid.Parse(result as string);

                                    var refNode = _contentBroker.GetNode(refNodeId);
                                    if (refNode.ActiveVersion.HasValue)
                                    {
                                        result = _contentBroker.GetNodeVersion(refNodeId, refNode.ActiveVersion.Value, TypeUtil.TypeFromName(refNode.Type));
                                    }
                                    else
                                    {
                                        result = null;
                                    }
                                }
                                else if (typeof(IEnumerable<ItemBase>).IsAssignableFrom(method.ReturnType))
                                {
                                    var refNodeIds = JsonConvert.DeserializeObject<IEnumerable<string>>(result as string);
                                    var refNodes = refNodeIds.Select(Guid.Parse).Select(_contentBroker.GetNode);

                                    result = refNodes
                                        .Where(ent => ent.ActiveVersion.HasValue)
                                        .Select(ent => _contentBroker.GetNodeVersion(ent.Id, ent.ActiveVersion.Value, TypeUtil.TypeFromName(ent.Type)));

                                    result = typeof(Enumerable)
                                        .GetMethod("Cast", new[] { typeof(IEnumerable) })
                                        .MakeGenericMethod(method.ReturnType.GenericTypeArguments.Single())
                                        .Invoke(null, new object[] { result });
                                }

                                handled = true;
                            }
                            break;
                    }
                }

                if (!handled)
                {
                    result = method.Invoke(_instance, args);
                }

                if (result == null && method.ReturnType.GetTypeInfo().IsValueType)
                {
                    result = Activator.CreateInstance(method.ReturnType);
                }

                if (result is Int64 && method.ReturnType == typeof(Int32))
                {
                    result = Convert.ToInt32(result);
                }

                return result;
            });
            throw new NotImplementedException();
        }

        public static ItemBase Create(Type type, IDictionary<string, object> innerDict, ContentBroker contentBroker)
        {
            var ret = typeof(DispatchProxy).GetMethod("Create").MakeGenericMethod(type, typeof(ItemBase)).Invoke(null, null) as ItemBase;

            ret._innerDict = innerDict;
            ret._contentBroker = contentBroker;

            ret._valueCache = new Dictionary<string, object>();

            return ret;
        }

        #region Private Fields

        static CamelCasePropertyNamesContractResolver NameResolver = new CamelCasePropertyNamesContractResolver();

        ContentBroker _contentBroker;
        IDictionary<string, object> _innerDict;
        object _instance;
        IDictionary<string, object> _valueCache;

        #endregion

        #region Private Methods

        object GetOrSetValue(string key, Func<object> generator)
        {
            if (!_valueCache.ContainsKey(key))
            {
                _valueCache[key] = generator();
            }

            return _valueCache[key];
        }

        #endregion
#endif
    }
}