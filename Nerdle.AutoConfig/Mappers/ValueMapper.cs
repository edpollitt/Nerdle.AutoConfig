using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    class ValueMapper : ISelectableMapper
    {
        public virtual object Map(XElement element, Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromInvariantString(element.Value);
        }

        public virtual bool CanMap(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.CanConvertFrom(typeof (string));
        }
    }
}