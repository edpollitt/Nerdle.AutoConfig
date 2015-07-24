using System;

namespace Nerdle.AutoConfig.Strategy
{
    interface IStrategyManager
    {
        void UpdateStrategy<T>(Action<IConfigureMappingStrategy<T>> configureMappingStrategy);
        IMappingStrategy GetStrategyFor<T>();
        IMappingStrategy GetStrategyFor(Type type);
    }
}