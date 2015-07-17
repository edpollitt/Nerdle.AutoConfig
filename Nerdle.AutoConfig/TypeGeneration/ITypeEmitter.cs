using System;

namespace Nerdle.AutoConfig.TypeGeneration
{
    interface ITypeEmitter
    {
        //Type GenerateInterfaceImplementation<TInterface>();
        Type GenerateInterfaceImplementation(Type interfaceType);
    }
}