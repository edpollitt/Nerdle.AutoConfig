using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Nerdle.AutoConfig.Exceptions;

namespace Nerdle.AutoConfig.Extensions
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
                throw new AutoConfigTypeGenerationException(string.Format("Cannot instantiate type '{0}' because it is not instantiable.", type.FullName));

            if (!type.HasParameterlessConstructor())
                throw new AutoConfigTypeGenerationException(string.Format("Cannot instantiate type '{0}' because no parameterless constructor was found.", type.FullName));

            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new AutoConfigTypeGenerationException(string.Format("An error occurred instantiating type '{0}'. See inner exception for details.", type.FullName), ex);
            }
        }

        public static IEnumerable<PropertyInfo> PublicSetters(this Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite);
        }

        public static string SectionName(this Type type)
        {
            var sectionName = type.Name;

            if (type.IsInterface && Regex.IsMatch(sectionName, "^I\\p{Lu}\\p{Ll}"))
                sectionName = sectionName.Substring(1);

            return sectionName;
        }

        public static Type GenericEnumerableType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                return type;

            return type.GetInterfaces()
                .Where(i => i.IsGenericType)
                .SingleOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}
