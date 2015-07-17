using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    class CollectionMapper : ISelectableMapper
    {
        public virtual object Map(XElement element, Type type)
        {
            if (!CanMap(type))
                throw new InvalidOperationException(
                    string.Format("Type '{0}' cannot be mapped by {1} because it cannot be assigned from a List<>.", type, GetType()));
            
            var listType = ListTypeFor(type);
            var genericArg = listType.GetGenericArguments().Single();
            
            var list = (IList)Activator.CreateInstance(listType);
            var itemMapper = MapperSelector.GetFor(genericArg);

            foreach (var child in element.Elements())
            {
                var item = itemMapper.Map(child, genericArg);
                list.Add(item);
            }

            return list;
        }

        public virtual bool CanMap(Type type)
        {
            var listType = ListTypeFor(type);
            return listType != null && type.IsAssignableFrom(listType);
        }

        static Type ListTypeFor(Type type)
        {
            var enumerableType = EnumerableTypeFor(type);

            if (enumerableType == null)
                return null;

            var genericArg = enumerableType.GetGenericArguments().Single();
            var listType = typeof(List<>).MakeGenericType(genericArg);

            return listType;
        }

        static Type EnumerableTypeFor(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type;

            return type.GetInterfaces()
                .Where(i => i.IsGenericType)
                .SingleOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}