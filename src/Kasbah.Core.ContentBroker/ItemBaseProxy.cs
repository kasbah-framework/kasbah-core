#if !DNXCORE50

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Kasbah.Core.ContentBroker.Models;
using Kasbah.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Kasbah.Core.ContentBroker
{
    public class ItemBaseProxy : RealProxy
    {
        #region Public Constructors

        public ItemBaseProxy(Type type, IDictionary<string, object> innerDict, ContentBroker contentBroker)
            : base(type)
        {
            _instance = Activator.CreateInstance(type);
            _innerDict = innerDict;
            _contentBroker = contentBroker;

            _valueCache = new Dictionary<string, object>();
        }

        #endregion

        #region Public Methods

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;

            try
            {
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

                                        result = typeof(System.Linq.Enumerable)
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
                        result = method.Invoke(_instance, methodCall.InArgs);
                    }

                    if (result == null && method.ReturnType.IsValueType)
                    {
                        result = Activator.CreateInstance(method.ReturnType);
                    }

                    if (result is Int64 && method.ReturnType == typeof(Int32))
                    {
                        result = Convert.ToInt32(result);
                    }

                    if (result is JArray && method.ReturnType != typeof(string))
                    {
                        var entType = method.ReturnType.GetGenericArguments().First();
                        var arrRes = Activator.CreateInstance(typeof(List<>).MakeGenericType(entType)) as IList;
                        foreach (var token in result as JArray)
                        {
                            var ent = token.ToObject(entType);
                            arrRes.Add(ent);
                        }
                        result = arrRes;
                    }

                    return result;
                });

                return new ReturnMessage(ret, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (TargetInvocationException ex)
            {
                return new ReturnMessage(ex.InnerException, msg as IMethodCallMessage);
            }
        }

        #endregion

        #region Private Fields

        static CamelCasePropertyNamesContractResolver NameResolver = new CamelCasePropertyNamesContractResolver();

        readonly ContentBroker _contentBroker;
        readonly IDictionary<string, object> _innerDict;
        readonly object _instance;
        readonly IDictionary<string, object> _valueCache;

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
    }
}

#endif