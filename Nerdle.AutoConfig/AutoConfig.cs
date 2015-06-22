using System.Configuration;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mappings;
using Nerdle.AutoConfig.TypeGeneration;

namespace Nerdle.AutoConfig
{
    public static class AutoConfig
    {
        public static T Map<T>()
        {
            var sectionName = typeof(T).SectionName();
            var section = ConfigurationManager.GetSection(sectionName) as Section;

            // TODO: instructions for overriding name and case, in the exception
            if (section == null)
                throw new AutoConfigMappingException(
                    string.Format("Could not load section '{0}'. Make sure the section exists and is correctly cased.", sectionName));

            var instance = TypeFactory.Create<T>();
            
            // since T might be an interface, we need the actual type
            var concreteType = instance.GetType();

            var mapping = TypeMapping.CreateFor(concreteType, section);
            mapping.Apply(instance);
            
            return instance;
        }
    }
}
