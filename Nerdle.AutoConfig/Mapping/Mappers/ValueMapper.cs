using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mapping.Mappers
{
    class ValueMapper : ISelectableMapper
    {
        public object Map(XElement element, Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromInvariantString(element.Value);
        }

        public bool CanMap(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.CanConvertFrom(typeof (string));
        }
    }
}