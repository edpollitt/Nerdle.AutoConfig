using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    class SimpletMapper : IQueryableMapper
    {
        public object Map(XElement element, Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromString(element.Value);
        }

        public bool CanMap(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.CanConvertFrom(typeof (string));
        }
    }
}