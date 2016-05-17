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

namespace Kasbah.Core.ContentBroker
{
    public class ItemBaseProxy
    {
        #region Public Constructors

        public ItemBaseProxy(Type type, IDictionary<string, object> innerDict, ContentBroker contentBroker)
        {
            _instance = Activator.CreateInstance(type);
            _innerDict = innerDict;
            _contentBroker = contentBroker;

            _valueCache = new Dictionary<string, object>();

            InitialiseInstance();
        }

        #endregion

        #region Public Methods

        public object GetTransparentProxy()
        {
            return _instance;
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

        object GetPropertyValue(string propertyName, Type returnType)
        {
            // TODO: make this great.
            var result = default(object);

            var type = _instance.GetType();
            var property = type.GetTypeInfo().GetProperties().Where(ent => ent.Name == propertyName).OrderBy(ent => ent.DeclaringType == type ? 0 : 1).First();

            var staticAttr = property.GetCustomAttribute<StaticAttribute>();
            if (staticAttr != null)
            {
                result = staticAttr.StaticValue;
            }
            else
            {
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

                            if (typeof(ItemBase).GetTypeInfo().IsAssignableFrom(returnType))
                            {
                                var refNodeId = Guid.Parse(result as string);

                                var refNode = _contentBroker.GetNode(refNodeId);
                                if (refNode.ActiveVersion.HasValue)
                                {
                                    result = _contentBroker.GetNodeVersion(refNodeId, refNode.ActiveVersion.Value, TypeUtil.TypeFromName(refNode.Type));
                                }
                                else
                                {
                                    // TODO: should this return null, or should an exception be raised?
                                    result = null;
                                }
                            }
                            else if (typeof(IEnumerable<ItemBase>).GetTypeInfo().IsAssignableFrom(returnType))
                            {
                                var refNodes = (result as JArray).ToObject<IEnumerable<Guid>>().Select(_contentBroker.GetNode);

                                result = refNodes
                                    .Where(ent => ent.ActiveVersion.HasValue)
                                    .Select(ent => _contentBroker.GetNodeVersion(ent.Id, ent.ActiveVersion.Value, TypeUtil.TypeFromName(ent.Type)));

                                // TODO: as above, should referenced values that doesn't have an active
                                //   version return null, or raise an exception?
                                result = typeof(Enumerable)
                                    .GetTypeInfo()
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
                    var entType = returnType.GetTypeInfo().GetGenericArguments().First();
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
                    var defaultAttr = property.GetCustomAttribute<DefaultAttribute>();
                    if (defaultAttr != null)
                    {
                        result = defaultAttr.DefaultValue;
                    }
                }
            }

            return result;
        }

        void InitialiseInstance()
        {
            var properties = _instance.GetType().GetRuntimeProperties();
            foreach (var property in properties)
            {
                if (property.SetMethod != null)
                {
                    var value = GetPropertyValue(property.Name, property.PropertyType);

                    property.SetValue(_instance, value);
                }
            }
        }

        #endregion
    }
}