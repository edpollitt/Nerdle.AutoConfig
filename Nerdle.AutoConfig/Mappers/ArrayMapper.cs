using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Nerdle.AutoConfig.Extensions;

namespace Nerdle.AutoConfig.Mappers
{
    class ArrayMapper : CollectionMapper
    {
        public override object Map(XElement element, Type type)
        {
            if (!type.IsArray)
                throw new InvalidOperationException(
                    string.Format("Type '{0}' does not implement Array.", type));

            if (type.GetArrayRank() > 1)
                throw new InvalidOperationException(
                   string.Format("Multidimensional arrays are not yet supported.", type));

            var arrayType = type.GetElementType();
            var toArray = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(arrayType);
            var collection = base.Map(element, type);
            var array = toArray.Invoke(null, new[] { collection });	
	        return array;
        }

        public override bool CanMap(Type type)
        {
            return type.IsArray && type.GetArrayRank() == 1;
        }
    }
}