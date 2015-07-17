using System;
using System.Collections.Concurrent;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;

namespace Nerdle.AutoConfig.TypeGeneration
{
    class TypeFactory : ITypeFactory
    {
        readonly ConcurrentDictionary<Type, Lazy<Type>> _typeDictionary = new ConcurrentDictionary<Type, Lazy<Type>>();
        readonly ITypeEmitter _emitter;

        public TypeFactory(ITypeEmitter emitter)
        {
            _emitter = emitter;
        }

        public T InstanceOf<T>()
        {
            return (T)InstanceOf(typeof(T));
        }

        public object InstanceOf(Type type)
        {
            EnsureSuitabilityOfType(type);

            if (type.IsInterface)
            {
                // GetOrAdd is not synchronised, using Lazy ensures the valueFactory is only ever called once.
                // See https://msdn.microsoft.com/en-us/library/dd997369.aspx
                var lazy = _typeDictionary.GetOrAdd(type,
                    new Lazy<Type>(() => _emitter.GenerateInterfaceImplementation(type)));

                return lazy.Value.Instantiate();
            }

            return type.Instantiate();
        }

        static void EnsureSuitabilityOfType(Type type)
        {
            if (type.IsValueType)
                throw new AutoConfigTypeGenerationException(
                    string.Format("Type '{0}' is a struct and is not supported. The requested configuration type should be an interface or class.", type.FullName));

            if (type.IsAbstract && !type.IsInterface)
                throw new AutoConfigTypeGenerationException(
                    string.Format("Cannot instantiate abstract class '{0}'.", type.FullName));
        }
    }
}
