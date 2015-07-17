using System;

namespace Nerdle.AutoConfig.TypeGeneration
{
    interface ITypeFactory
    {
        object InstanceOf(Type type);
    }
}