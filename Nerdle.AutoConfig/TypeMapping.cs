using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig
{
    class TypeMapping<T> : TypeMapping
    {
        readonly IList<PropertyMapping<T>> _propertyMappings = new List<PropertyMapping<T>>();

        public void Include(PropertyMapping<T> propertyMapping)
        {
            _propertyMappings.Add(propertyMapping);
        }

        public void Apply(T instance)
        {
            foreach (var mapping in _propertyMappings)
            {
                mapping.Apply(instance);
            }
        }
    }

    abstract class TypeMapping
    {
        public static TypeMapping<T> CreateFor<T>(XElement sectionElement)
        {
            var typeMapping = new TypeMapping<T>();

            var elements = sectionElement.Descendants();
            var properties = typeof(T).PublicSetters().ToList();

            foreach (var element in elements)
            {
                var property = properties.FirstOrDefault(p => p.Name == element.Name.LocalName);

                if (property == null)
                    throw new AutoConfigMappingException(
                        string.Format("Could not map type {0} from section '{1}'. No matching property for config element '{2}' was founnd.",
                            typeof(T), sectionElement.Name.LocalName, element.Name.LocalName));

                var propertyMapping = new PropertyMapping<T>(element, property, new BasicMapper<T>());

                typeMapping.Include(propertyMapping);
                properties.Remove(property);
            }

            if (properties.Any())
                throw new AutoConfigMappingException(
                     string.Format("Could not map property '{0}' for type {1} from section '{2}'. No matching config element was founnd.",
                     properties.First().Name, typeof(T), sectionElement.Name.LocalName));

            return typeMapping;
        }
    }
}
