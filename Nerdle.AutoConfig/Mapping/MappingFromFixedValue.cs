using System.Reflection;

namespace Nerdle.AutoConfig.Mapping
{
    class MappingFromFixedValue : IPropertyMapping
    {
        readonly PropertyInfo _property;
        readonly object _fixedValue;

        public MappingFromFixedValue(PropertyInfo property, object fixedValue)
        {
            _property = property;
            _fixedValue = fixedValue;
        }

        public void Apply(object instance)
        {
            _property.SetValue(instance, _fixedValue, null);
        }
    }
}