using System;
using System.Collections.Concurrent;

namespace Nerdle.AutoConfig.Strategy
{
    class StrategyManager : IStrategyManager
    {
        static readonly ConcurrentDictionary<Type, IMappingStrategy> Strategies = new ConcurrentDictionary<Type, IMappingStrategy>();

        public static readonly IMappingStrategy DefaultStrategy = new MappingStrategy();

        public void UpdateStrategy<T>(Action<IConfigureMappingStrategy<T>> strategyConfiguration)
        { 
            Strategies.AddOrUpdate(typeof(T),
                type =>
                {
                    var strategy = new ConfigureMappingStrategy<T>();
                    strategyConfiguration(strategy);
                    return strategy;
                },
                (type, cfg) =>
                {
                    var config = cfg as ConfigureMappingStrategy<T>;
                    strategyConfiguration(config);
                    return config;
                });
        }

        public IMappingStrategy GetStrategyFor<T>()
        {
            return GetStrategyFor(typeof(T));
        }

        public IMappingStrategy GetStrategyFor(Type type)
        {
            IMappingStrategy strategy;
            return Strategies.TryGetValue(type, out strategy) ? strategy : DefaultStrategy;
        }
    }
}