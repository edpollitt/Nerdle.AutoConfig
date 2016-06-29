using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    class KeyValuePairMapper : ISelectableMapper
    {
        public object Map(XElement element, Type type)
        {
            if (!CanMap(type))
                throw new InvalidOperationException(
                    string.Format("Type '{0}' cannot be mapped by {1} because it is not a KeyValuePair<,>.", type, GetType()));
       
            var typeArgs = type.GetGenericArguments();
            var keyValuePairBuilderType = typeof(KeyValuePairBuilder<,>).MakeGenericType(typeArgs);
            var keyValuePairBuilderInstance = AutoConfig.Map(keyValuePairBuilderType, element);
            var keyValuePair = keyValuePairBuilderType.GetMethod("Build").Invoke(keyValuePairBuilderInstance, null);
            return keyValuePair;
        }

        public bool CanMap(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
        }

        // Helper class because KeyValuePair is immutable so we can't create it using property mapping
        class KeyValuePairBuilder<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }

            public KeyValuePair<TKey, TValue> Build()
            {
                return new KeyValuePair<TKey, TValue>(Key, Value);
            }
        }
    }
}
