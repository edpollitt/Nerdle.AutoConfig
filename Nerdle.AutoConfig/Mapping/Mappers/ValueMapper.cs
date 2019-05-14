using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mapping.Mappers
{
    class ValueMapper : ISelectableMapper
    {
        public object Map(XElement element, Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            var result = converter.ConvertFromInvariantString(element.Value);
            if (type.IsEnum && result != null && !type.IsEnumDefined(result))
            {
                var definedValues = Enum.GetValues(type).Cast<object>().Select(e => e.ToString());
                throw new ArgumentOutOfRangeException(nameof(element), result, $"Failed to convert '{element.Value}' into '{type}' because it is not a defined value of the enum type. Defined values: '{string.Join("', '", definedValues)}'");
            }
            return result;
        }

        public bool CanMap(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.CanConvertFrom(typeof (string));
        }
    }
}