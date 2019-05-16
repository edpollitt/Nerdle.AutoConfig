using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanWrite && p.GetSetMethod(true).IsPublic);
        }

        public static string ConcreteName(this Type type)
        {
            var sectionName = type.Name;

            if (type.IsInterface && Regex.IsMatch(sectionName, "^I\\p{Lu}\\p{Ll}"))
                sectionName = sectionName.Substring(1);

            return sectionName;
        }

        public static object ConvertFromInvariantString(this Type type, string value)
        {
            var converter = TypeDescriptor.GetConverter(type);
            var result = converter.ConvertFromInvariantString(value);
            if (type.IsEnum && result != null && !type.IsEnumDefined(result))
            {
                var definedValues = Enum.GetValues(type).Cast<object>().Select(e => e.ToString());
                throw new ArgumentOutOfRangeException(nameof(value), result, $"Failed to convert '{value}' into '{type}' because it is not a defined value of the enum type. Defined values: '{string.Join("', '", definedValues)}'");
            }
            return result;
        }
    }
}
