using System;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mapping.Mappers
{
    class RecursingMapper : IMapper
    {
        public object Map(XElement element, Type type)
        {
            return AutoConfig.Map(type, element);
        }
    }
}
