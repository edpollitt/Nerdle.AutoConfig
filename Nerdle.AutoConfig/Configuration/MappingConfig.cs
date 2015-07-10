using System.Collections.Concurrent;
using Nerdle.AutoConfig.Casing;

namespace Nerdle.AutoConfig.Configuration
{
    class MappingConfig
    {
        public ConcurrentDictionary<string, PropertyMappingConfig> PropertyConfigs { get; private set; }
        public ICase Case { get; protected set; }

        public MappingConfig()
        {
            Case = new CamelCase();
            PropertyConfigs = new ConcurrentDictionary<string, PropertyMappingConfig>();
        }
    }
}