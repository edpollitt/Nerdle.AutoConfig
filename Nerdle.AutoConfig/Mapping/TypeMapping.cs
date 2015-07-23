using System.Collections.Generic;

namespace Nerdle.AutoConfig.Mapping
{
    class TypeMapping
    {
        readonly List<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public void Include(IPropertyMapping propertyMapping)
        {
            _propertyMappings.Add(propertyMapping);
        }

        public void Apply(object instance)
        {
            _propertyMappings.ForEach(mapping => mapping.Apply(instance));
        }
    }
}
