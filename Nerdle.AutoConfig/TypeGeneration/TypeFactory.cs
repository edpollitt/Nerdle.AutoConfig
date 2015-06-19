using System;
using System.Collections.Concurrent;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;

namespace Nerdle.AutoConfig.TypeGeneration
{
    static class TypeFactory
    {
        static readonly ConcurrentDictionary<Type, Lazy<Type>> TypeDictionary = new ConcurrentDictionary<Type, Lazy<Type>>();

        public static T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        static object Create(Type type)
        {
            EnsureTypeSuitability(type);

            if (type.IsInterface)
            {
                // GetOrAdd is not synchronised, using Lazy ensures the valueFactory is only ever called once.
                // See https://msdn.microsoft.com/en-us/library/dd997369.aspx
                var lazy = TypeDictionary.GetOrAdd(type,
                    new Lazy<Type>(() => TypeEmitter.GenerateInterfaceImplementation(type)));

                return lazy.Value.Instantiate();
            }

            return type.Instantiate();
        }

        static void EnsureTypeSuitability(Type type)
        {
            if (type.IsValueType)
                throw new AutoConfigTypeGenerationException(
                    string.Format("Type {0} is a struct and is not supported. The requested configuration type should be an interface or class.", type.FullName));

            if (type.IsAbstract && !type.IsInterface)
                throw new AutoConfigTypeGenerationException(
                    string.Format("Cannot instantiate abstract class {0}.", type.FullName));
        }
    }
}
