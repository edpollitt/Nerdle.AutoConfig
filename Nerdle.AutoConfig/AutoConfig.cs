using System;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mapping;
using Nerdle.AutoConfig.Sections;
using Nerdle.AutoConfig.Strategy;
using Nerdle.AutoConfig.TypeGeneration;

namespace Nerdle.AutoConfig
{
    public static class AutoConfig
    {
        static readonly MappingEngine Engine;

        static AutoConfig()
        {
            Engine = new MappingEngine(
                new SectionProvider(new DefaultSectionNameConvention(), new ConfigurationSystem()), 
                new TypeFactory(new TypeEmitter()), 
                new MappingFactory(), 
                new StrategyManager());
        }

        public static T Map<T>(string sectionName = null)
        {
            return Engine.Map<T>(sectionName);
        }

        public static T MapWithDefault<T>(T defaultConfiguration, string sectionName = null)
        {
            try
            {
                return Map<T>(sectionName);
            }
            catch (AutoConfigMappingException)
            {
                return defaultConfiguration;
            }
        }

        internal static object Map(Type type, XElement element)
        {
           return Engine.Map(type, element);
        }

        public static void WhenMapping<T>(Action<IConfigureMappingStrategy<T>> configureMapping)
        {
            Engine.WhenMapping(configureMapping);
        }
    }
}
