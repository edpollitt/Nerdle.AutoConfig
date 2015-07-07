using System;
using System.Configuration;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mappings;
using Nerdle.AutoConfig.TypeGeneration;

namespace Nerdle.AutoConfig
{
    public static class AutoConfig
    {
        public static T Map<T>(string sectionName = null)
        {
            sectionName = sectionName ?? typeof(T).SectionName();
            var section = ConfigurationManager.GetSection(sectionName) as Section;

            if (section == null)
                throw new AutoConfigMappingException(
                    string.Format("Could not load section '{0}'. Make sure the section exists and is correctly cased.", sectionName));

            return Map<T>(section);
        }

        internal static T Map<T>(XElement element)
        {
            return (T)Map(typeof (T), element);
        }

        internal static object Map(Type type, XElement element)
        {
            var instance = TypeFactory.Create(type);

            // since T might be an interface, we need the actual type
            var concreteType = instance.GetType();

            var mapping = TypeMapping.CreateFor(concreteType, element);
            mapping.Apply(instance);

            return instance;
        }
    }
}
