using System;
using Nerdle.AutoConfig.Exceptions;

namespace Nerdle.AutoConfig.TypeGeneration.Extensions
{
    static class TypeExtensions
    {
        public static bool IsInstantiable(this Type type)
        {
            return !type.IsInterface && !type.IsAbstract;
        }

        public static bool HasParameterlessConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        public static object Instantiate(this Type type)
        {
            if (!type.IsInstantiable())
                throw new AutoConfigTypeGenerationException(string.Format("Cannot instantiate type {0} because it is not instantiable.", type.FullName));

            if (!type.HasParameterlessConstructor())
                throw new AutoConfigTypeGenerationException(string.Format("Cannot instantiate type {0} because no parameterless constructor was found.", type.FullName));

            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new AutoConfigTypeGenerationException(string.Format("An error occurred instantiating type {0}. See inner exception for details.", type.FullName), ex);
            }
        }
    }
}
