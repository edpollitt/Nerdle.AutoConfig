using System;
using System.Linq;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mappers;
using Nerdle.AutoConfig.Strategy;

namespace Nerdle.AutoConfig.Mapping
{
    class  MappingFactory : IMappingFactory
    {
        public TypeMapping CreateMapping(Type type, XElement sectionElement, IMappingStrategy mappingStrategy)
        {
            var typeMapping = new TypeMapping();
            var properties = type.PublicSetters().ToList();

            foreach (var element in sectionElement.Elements())
            {
                var property = properties.FirstOrDefault(p => mappingStrategy.NameFor(p) == element.Name.LocalName);

                if (property == null)
                    throw new AutoConfigMappingException(
                        string.Format("Could not map type '{0}' from section '{1}'. No matching settable property for config element '{2}' was found.",
                            type, sectionElement.Name.LocalName, element.Name.LocalName));

                var mapper = MapperSelector.GetFor(property.PropertyType);
                var propertyMapping = new MappingFromElement(element, property, mapper);

                typeMapping.Include(propertyMapping);
                properties.Remove(property);
            }

            foreach (var attribute in sectionElement.Attributes())
            {
                var property = properties.FirstOrDefault(p => mappingStrategy.NameFor(p) == attribute.Name.LocalName);

                if (property == null)
                    throw new AutoConfigMappingException(
                        string.Format("Could not map type '{0}' from section '{1}'. No matching settable property for config attribute '{2}' was found.",
                            type, sectionElement.Name.LocalName, attribute.Name.LocalName));

                var propertyMapping = new MappingFromAttribute(attribute, property);

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