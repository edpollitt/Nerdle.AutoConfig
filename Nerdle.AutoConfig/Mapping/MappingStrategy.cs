using System.Collections.Concurrent;
using Nerdle.AutoConfig.Casing;
using Nerdle.AutoConfig.Strategy;

namespace Nerdle.AutoConfig.Mapping
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
    }
}