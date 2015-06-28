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
            if (!CanMap(type))
                throw new InvalidOperationException(
                    string.Format("Type '{0}' cannot be mapped by {1} because it is not a single dimensional Array.", type, GetType()));
            
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