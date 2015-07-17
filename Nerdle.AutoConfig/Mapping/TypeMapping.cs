using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mappers;

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
