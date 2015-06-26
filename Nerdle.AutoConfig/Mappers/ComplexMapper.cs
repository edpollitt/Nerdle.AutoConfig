using System;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    class ComplexMapper : IMapper
    {
        public virtual object Map(XElement element, Type type)
        {
            return AutoConfig.Map(type, element);
        }

        // TODO: Not sure we can take this approach, too hard to provide a consistent and complete solution?
        //public bool CanMap(Type type)
        //{
            
        //    if (typeof(ValueType).IsAssignableFrom(type)
        //     || typeof(IConvertible).IsAssignableFrom(type))
        //        return false;

        //    return type.GenericEnumerableType() == null;
        //}
    }
}
