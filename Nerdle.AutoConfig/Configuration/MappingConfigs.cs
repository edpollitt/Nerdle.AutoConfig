using System;
using System.Collections.Concurrent;

namespace Nerdle.AutoConfig.Configuration
{
    class MappingConfigs
    {
        static readonly ConcurrentDictionary<Type, MappingConfig> Configs = new ConcurrentDictionary<Type, MappingConfig>();

        public static readonly MappingConfig Default = new MappingConfig();

        public static void Apply<T>(Action<IConfigureMapping<T>> configureMapping)
        {
            Configs.AddOrUpdate(typeof(T),
                type =>
                {
                    var config = new ConfigureMapping<T>();
                    configureMapping(config);
                    return config;
                },
                (type, cfg) =>
                {
                    var config = cfg as ConfigureMapping<T>;
                    configureMapping(config);
                    return config;
                });
        }

        public static MappingConfig GetFor<T>()
        {
            return GetFor(typeof(T));
        }

        public static MappingConfig GetFor(Type type)
        {
            MappingConfig config;
            return Configs.TryGetValue(type, out config) ? config : Default;
        }
    }
}