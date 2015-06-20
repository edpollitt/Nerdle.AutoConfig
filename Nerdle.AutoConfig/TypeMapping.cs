using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig
{
    class TypeMapping
    {
        readonly IList<PropertyMapping> _propertyMappings = new List<PropertyMapping>();

        public void Include(PropertyMapping propertyMapping)
        {
            _propertyMappings.Add(propertyMapping);
        }

        public void Apply(object instance)
        {
            foreach (var mapping in _propertyMappings)
            {
                mapping.Apply(instance);
            }
        }

        public static TypeMapping CreateFor(Type type, XElement sectionElement)
        {
            var typeMapping = new TypeMapping();
            var elements = sectionElement.Elements();
            var properties = type.PublicSetters().ToList();

            foreach (var element in elements)
            {
                var property = properties.FirstOrDefault(p => p.Name == element.Name.LocalName);

                if (property == null)
                    throw new AutoConfigMappingException(
                        string.Format("Could not map type {0} from section '{1}'. No matching settable property for config element '{2}' was founnd.",
                            type, sectionElement.Name.LocalName, element.Name.LocalName));

                var mapper = Mapper.For(property.PropertyType);
                var propertyMapping = new PropertyMapping(element, property, mapper);

                typeMapping.Include(propertyMapping);
                properties.Remove(property);
            }

            if (properties.Any())
                throw new AutoConfigMappingException(
                     string.Format("Could not map property '{0}' for type {1} from section '{2}'. No matching config element was founnd.",
                     properties.First().Name, type, sectionElement.Name.LocalName));

            return typeMapping;
        }
    }
}
