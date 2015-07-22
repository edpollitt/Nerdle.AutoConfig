using System.Collections.Concurrent;
using Nerdle.AutoConfig.Casing;
using Nerdle.AutoConfig.Strategy;
using System.Reflection;
using System;

namespace Nerdle.AutoConfig.Strategy
{
    class MappingStrategy : IMappingStrategy
    {
        public ConcurrentDictionary<string, PropertyStrategy> PropertyStrategies { get; private set; }
      
        public ICaseConverter CaseConverter { get; protected set; }

        public MappingStrategy()
        {
            CaseConverter = new CamelCaseConverter();
            //PropertyConfigs = new ConcurrentDictionary<string, PropertyMappingConfig>();
        }

        public string ConvertCase(string s)
        {
            return CaseConverter.Convert(s);
        }

        public string NameFor(PropertyInfo property)
        {
            return ConvertCase(property.Name);
        }
    }
}