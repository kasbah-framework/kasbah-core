using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kasbah.Core.Annotations;
using Kasbah.Core.ContentBroker.Models;
using Kasbah.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

#if !DNXCORE50

using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

#endif

namespace Kasbah.Core.ContentBroker
{
    public class ItemBaseProxy
#if !DNXCORE50
        : RealProxy
#endif
    {
        #region Public Constructors

        public ItemBaseProxy(Type type, IDictionary<string, object> innerDict, ContentBroker contentBroker)
#if !DNXCORE50
            : base(type)
#endif
        {
            _instance = Activator.CreateInstance(type);
            _innerDict = innerDict;
            _contentBroker = contentBroker;

            _valueCache = new Dictionary<string, object>();

#if DNXCORE50
            InitialiseInstance();
#endif
        }

        #endregion

        #region Public Methods

#if DNXCORE50
        public object GetTransparentProxy()
        {
            return _instance;
        }
#else

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            var method = (MethodInfo)methodCall.MethodBase;

            try
            {
                var result = default(object);
                var handled = false;
                if (method.Name.StartsWith("get_"))
                {
                    result = GetOrSetValue(method.Name, () =>
                     {
                         return GetPropertyValue(method.Name.Substring("get_".Length), method.ReturnType);
                     });

                    handled = result != null;
                }

                if (!handled)
                {
                    result = method.Invoke(_instance, methodCall.InArgs);
                }

                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (TargetInvocationException ex)
            {
                return new ReturnMessage(ex.InnerException, msg as IMethodCallMessage);
            }
        }

#endif

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

        object GetPropertyValue(string propertyName, Type returnType)
        {
            // TODO: make this great.
            var result = default(object);
            var name = NameResolver.GetResolvedPropertyName(propertyName);

            switch (name)
            {
                case "node":
                    {
                        var id = Guid.Parse(_innerDict["id"] as string);

                        result = _contentBroker.GetNode(id);
                    }
                    break;

                default:
                    if (_innerDict.ContainsKey(name))
                    {
                        result = _innerDict[name];

                        if (typeof(ItemBase).IsAssignableFrom(returnType))
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
                        else if (typeof(IEnumerable<ItemBase>).IsAssignableFrom(returnType))
                        {
                            var refNodes = (result as JArray).ToObject<IEnumerable<Guid>>().Select(_contentBroker.GetNode);

                            result = refNodes
                                .Where(ent => ent.ActiveVersion.HasValue)
                                .Select(ent => _contentBroker.GetNodeVersion(ent.Id, ent.ActiveVersion.Value, TypeUtil.TypeFromName(ent.Type)));

                            result = typeof(Enumerable)
                                .GetMethod("Cast", new[] { typeof(IEnumerable) })
                                .MakeGenericMethod(returnType.GenericTypeArguments.Single())
                                .Invoke(null, new object[] { result });
                        }
                    }
                    break;
            }

            if (result == null && returnType.GetTypeInfo().IsValueType)
            {
                result = Activator.CreateInstance(returnType);
            }

            if (result is Int64 && returnType == typeof(Int32))
            {
                result = Convert.ToInt32(result);
            }

            if (result is JArray && returnType != typeof(string))
            {
                var entType = returnType.GetGenericArguments().First();
                var arrRes = Activator.CreateInstance(typeof(List<>).MakeGenericType(entType)) as IList;
                foreach (var token in result as JArray)
                {
                    var ent = token.ToObject(entType);
                    arrRes.Add(ent);
                }
                result = arrRes;
            }

            if (result is string && returnType != typeof(string))
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result as string, returnType);
                }
                catch (JsonReaderException)
                {
                    result = JsonConvert.DeserializeObject($"'{result}'", returnType);
                }
            }

            if (result == null)
            {
                var defaultAttr = _instance.GetType().GetProperty(propertyName).GetCustomAttribute<DefaultAttribute>();
                if (defaultAttr != null)
                {
                    result = defaultAttr.DefaultValue;
                }
            }

            return result;
        }

#if DNXCORE50
        void InitialiseInstance()
        {
            var properties = _instance.GetType().GetAllProperties();
            foreach (var property in properties)
            {
                if (property.SetMethod != null)
                {
                    var value = GetPropertyValue(property.Name, property.PropertyType);

                    property.SetValue(_instance, value);
                }
            }
        }
#endif

        #endregion
    }
}