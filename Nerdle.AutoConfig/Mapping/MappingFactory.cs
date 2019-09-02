using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mapping.Mappers;
using Nerdle.AutoConfig.Strategy;

namespace Nerdle.AutoConfig.Mapping
{
    class  MappingFactory : IMappingFactory
    {
        public ITypeMapping CreateMapping(Type type, XElement sectionElement, IMappingStrategy mappingStrategy)
        {
            var typeMapping = new TypeMapping();

            var elementList = sectionElement.Elements().ToList();
            var attributeList = sectionElement.Attributes().ToList();

            foreach (var property in type.PublicSetters().ToList())
            {
                var propertyStrategy = mappingStrategy.ForProperty(property);

                var xName = propertyStrategy.MapFrom ?? mappingStrategy.ConvertCase(property.Name);

                if (TryAddPropertyFromElement(typeMapping, elementList, property, propertyStrategy, xName))
                    continue;

                if (TryAddPropertyFromAttribute(type, typeMapping, attributeList, property, propertyStrategy, xName))
                    continue;

                if (!propertyStrategy.IsOptional)
                    throw new AutoConfigMappingException(
                        string.Format(
                            "Could not map property '{0}' for type '{1}' from section '{2}'. Looked for a config element or attribute with name '{3}' but didn't find one, and the property is not marked as optional.",
                            property.Name, type, sectionElement.Name.LocalName, xName));

                var defaultValue = propertyStrategy.DefaultValue; 
                if (defaultValue != null)
                {
                    EnsureDefaultValueType(ref defaultValue, property, type, sectionElement.Name.LocalName);
                    var propertyMapping = new MappingFromFixedValue(property, defaultValue);
                    typeMapping.Include(propertyMapping);
                }
            }

            EnsureEmpty(elementList, element => string.Format(
                "Could not map type '{0}' from section '{1}'. No matching settable property for config element '{2}' was found.",
                type, sectionElement.Name.LocalName, element.Name.LocalName));

            EnsureEmpty(attributeList, attribute => string.Format(
                "Could not map type '{0}' from section '{1}'. No matching settable property for config attribute '{2}' was found.",
                type, sectionElement.Name.LocalName, attribute.Name.LocalName));

            return typeMapping;
        }

        static bool TryAddPropertyFromElement(ITypeMapping typeMapping, ICollection<XElement> elementList, PropertyInfo property, IPropertyStrategy propertyStrategy, string name)
        {
            var element = TakeElement(elementList, name);
            
            if (element == null) 
                return false;
            
            var mapper = propertyStrategy.Mapper ?? MapperSelector.GetFor(property.PropertyType);
            var propertyMapping = new MappingFromElement(element, property, mapper);
            typeMapping.Include(propertyMapping);
            return true;
        }

        static bool TryAddPropertyFromAttribute(Type type, ITypeMapping typeMapping, ICollection<XAttribute> attributeList, PropertyInfo property, IPropertyStrategy propertyStrategy, string name)
        {
            var attribute = TakeAttribute(attributeList, name);
            
            if (attribute == null)
                return false;
            
            if (propertyStrategy.Mapper != null)
                throw new AutoConfigMappingException(
                    string.Format(
                        "Custom IMapper '{0}' was configured for mapping property '{1}' of '{2}'. A custom IMapper cannot be specified when mapping from an attribute.",
                        propertyStrategy.Mapper, property.Name, type));

            var propertyMapping = new MappingFromAttribute(attribute, property);
            typeMapping.Include(propertyMapping);

            return true;
        }

        static XElement TakeElement(ICollection<XElement> elementList, string name)
        {
            return TakeSingleMatching(elementList, attr => attr.Name == name,
              count => string.Format("Found {0} elements with name '{1}' but only expected to find one.", count, name));
        }

        static XAttribute TakeAttribute(ICollection<XAttribute> attributeList, string name)
        {
            return TakeSingleMatching(attributeList, attr => attr.Name == name,
                count => string.Format("Found {0} attributes with name '{1}' but only expected to find one.", count, name));
        }

        static T TakeSingleMatching<T>(ICollection<T> collection, Func<T, bool> predicate, Func<int, string> exceptionMessage)
        {
            var matching = collection.Where(predicate).ToList();

            if (matching.Count() > 1)
                throw new AutoConfigMappingException(exceptionMessage(matching.Count()));

            var matched = matching.SingleOrDefault();

            if (matched != null)
                collection.Remove(matched);

            return matched;
        }

        static void EnsureEmpty<T>(ICollection<T> collection, Func<T, string> exceptionMessage)
        {
            if (collection.Any())
            {
                throw new AutoConfigMappingException(exceptionMessage(collection.First()));
            }
        }

        static void EnsureDefaultValueType(ref object defaultValue, PropertyInfo property, Type type, string sectionName)
        {
            var defaultValueType = defaultValue.GetType(); 
            if (!property.PropertyType.IsAssignableFrom(defaultValueType))
            {
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                if (!converter.CanConvertFrom(defaultValueType))
                {
                    throw new AutoConfigMappingException(
                        string.Format(
                            "Could not map property '{0}' for type '{1}' from section '{2}'. The default value '{3}' of type '{4}' can not be converted to '{5}'.",
                            property.Name, type, sectionName, defaultValue, defaultValueType, property.PropertyType));
                }
                try
                {
                    defaultValue = converter.ConvertFrom(defaultValue);
                }
                catch (Exception ex)
                {
                    throw new AutoConfigMappingException(
                        string.Format(
                            "Could not map property '{0}' for type '{1}' from section '{2}'. The default value '{3}' failed to be converted to '{4}'.",
                            property.Name, type, sectionName, defaultValue, property.PropertyType), ex);
                }
            }
        }
    }
}