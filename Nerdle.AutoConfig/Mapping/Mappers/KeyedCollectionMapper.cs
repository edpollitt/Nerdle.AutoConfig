using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mapping.Mappers
{
    class KeyedCollectionMapper : CollectionMapper
    {
        public override object Map(XElement element, Type type)
        {
            if (!CanMap(type))
                throw new InvalidOperationException(
                    string.Format("Type '{0}' cannot be mapped by {1} because it is not a KeyedCollection.", type, GetType()));

            if (!type.IsInstantiable())
                throw new AutoConfigMappingException(string.Format("Cannot map to type '{0}' because it is not instantiable.", type.FullName));

            if (!type.HasParameterlessConstructor())
                throw new AutoConfigMappingException(string.Format("Cannot map to type '{0}' because no parameterless constructor was found.", type.FullName));

            var values = base.Map(element, type) as IEnumerable;

            var keyedCollection = Activator.CreateInstance(type);
            var addMethod = type.GetMethod("Add");

            foreach (var value in values)
            {
                addMethod.Invoke(keyedCollection, new[] { value });
            }

            return keyedCollection;
        }

        public override bool CanMap(Type type)
        {
            return IsKeyedCollection(type);
        }

        static bool IsKeyedCollection(Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyedCollection<,>))
                    return true;

                type = type.BaseType;
            }

            return false;
        }
    }
}