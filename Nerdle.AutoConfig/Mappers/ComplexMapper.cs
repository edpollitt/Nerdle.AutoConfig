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
    }
}
