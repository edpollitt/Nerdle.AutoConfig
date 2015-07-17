using System;
using System.Collections.Concurrent;
using Nerdle.AutoConfig.Mapping;

namespace Nerdle.AutoConfig.Strategy
{
    class MappingStrategyProvider : IMappingStrategyProvider
    {
        static readonly ConcurrentDictionary<Type, IMappingStrategy> Strategies = new ConcurrentDictionary<Type, IMappingStrategy>();

        public static readonly IMappingStrategy Default = new MappingStrategy();

        public static void Apply<T>(Action<IConfigureMappingStrategy<T>> configureStrategy)
        {
            Strategies.AddOrUpdate(typeof(T),
                type =>
                {
                    var strategy = new ConfigureMappingStrategy<T>();
                    configureStrategy(strategy);
                    return strategy;
                },
                (type, cfg) =>
                {
                    var config = cfg as ConfigureMappingStrategy<T>;
                    configureStrategy(config);
                    return config;
                });
        }

        public IMappingStrategy GetFor<T>()
        {
            return GetFor(typeof(T));
        }

        public IMappingStrategy GetFor(Type type)
        {
            IMappingStrategy strategy;
            return Strategies.TryGetValue(type, out strategy) ? strategy : Default;
        }
    }
}