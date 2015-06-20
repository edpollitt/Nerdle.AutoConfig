using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Nerdle.AutoConfig.Extensions;

namespace Nerdle.AutoConfig.Mappers
{
    class CollectionMapper : IQueryableMapper
    {
        public object Map(XElement element, Type type)
        {
            var enumerableType = type.GenericEnumerableType();

            if (enumerableType == null)
                throw new InvalidOperationException(
                    string.Format("Type {0} does not implement IEnumerable<>.", type));

            var genericType = enumerableType.GetGenericArguments().Single();
            var listType = typeof(List<>).MakeGenericType(genericType);
            var list = Activator.CreateInstance(listType) as IList;

            var itemMapper = Mapper.For(genericType);

            foreach (var child in element.Elements())
            {
                var item = itemMapper.Map(child, genericType);
                list.Add(item);
            }

            return list;
        }

        public bool CanMap(Type type)
        {
            var enumerableType = type.GenericEnumerableType();
   
            if (enumerableType == null)
                return false;

            var genericType = enumerableType.GetGenericArguments().Single();
            var listType = typeof(List<>).MakeGenericType(genericType);

            return type.IsAssignableFrom(listType);
        }
    }
}