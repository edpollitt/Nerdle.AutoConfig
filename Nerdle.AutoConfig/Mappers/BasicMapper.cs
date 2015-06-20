using System;
using System.Reflection;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    class BasicMapper : IMapper
    {
        public void Map(XElement element, PropertyInfo property, object instance)
        {
            var value = Convert.ChangeType(element.Value, property.PropertyType);
            property.SetValue(instance, value, null);
        }
    }
}