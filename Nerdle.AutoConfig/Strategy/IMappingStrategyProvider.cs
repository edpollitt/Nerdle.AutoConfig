using System;

namespace Nerdle.AutoConfig.Strategy
{
    interface IMappingStrategyProvider
    {
        IMappingStrategy GetFor<T>();
        IMappingStrategy GetFor(Type type);
    }
}