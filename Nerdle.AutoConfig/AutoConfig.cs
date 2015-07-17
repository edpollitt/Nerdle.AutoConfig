using System;
using System.Xml.Linq;
using Nerdle.AutoConfig.Strategy;

namespace Nerdle.AutoConfig
{
    public static class AutoConfig
    {
        static readonly MappingEngine Engine;

        static AutoConfig()
        {
            Engine = new MappingEngine(null, null, null, null);
        }

        public static T Map<T>(string sectionName = null)
        {
            return Engine.Map<T>(sectionName);
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
