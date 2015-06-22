using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig.Mappings
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

        public static TypeMapping CreateFor(Type type, XElement sectionElement)
        {
            var typeMapping = new TypeMapping();
            var properties = type.PublicSetters().ToList();
            
            foreach (var element in sectionElement.Elements())
            {
                var property = properties.FirstOrDefault(p => p.Name == element.Name.LocalName);

                if (property == null)
                    throw new AutoConfigMappingException(
                        string.Format("Could not map type '{0}' from section '{1}'. No matching settable property for config element '{2}' was found.",
                            type, sectionElement.Name.LocalName, element.Name.LocalName));

                var mapper = Mapper.For(property.PropertyType);
                var propertyMapping = new ElementMapping(element, property, mapper);

                typeMapping.Include(propertyMapping);
                properties.Remove(property);
            }

            foreach (var attribute in sectionElement.Attributes())
            {
                var property = properties.FirstOrDefault(p => p.Name == attribute.Name.LocalName);

                if (property == null)
                    throw new AutoConfigMappingException(
                        string.Format("Could not map type '{0}' from section '{1}'. No matching settable property for config attribute '{2}' was found.",
                            type, sectionElement.Name.LocalName, attribute.Name.LocalName));

                var propertyMapping = new AttributeMapping(attribute, property);

                typeMapping.Include(propertyMapping);
                properties.Remove(property);
            }

            if (properties.Any())
                throw new AutoConfigMappingException(
                     string.Format("Could not map property '{0}' for type '{1}' from section '{2}'. No matching config element or attribute was found.",
                     properties.First().Name, type, sectionElement.Name.LocalName));

            return typeMapping;
        }
    }
}
