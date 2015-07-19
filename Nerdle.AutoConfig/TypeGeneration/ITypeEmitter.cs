using System;

namespace Nerdle.AutoConfig.TypeGeneration
{
    interface ITypeEmitter
    {
        Type GenerateInterfaceImplementation(Type interfaceType);
    }
}