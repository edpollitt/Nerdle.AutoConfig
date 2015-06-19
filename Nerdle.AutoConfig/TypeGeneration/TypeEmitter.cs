using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;

namespace Nerdle.AutoConfig.TypeGeneration
{
    class TypeEmitter
    {
        public const string AssemblyName = "AutoConfig.DynamicTypes";

        public static Type GenerateInterfaceImplementation<TInterface>()
        {
            return GenerateInterfaceImplementation(typeof(TInterface));
        }

        public static Type GenerateInterfaceImplementation(Type interfaceType)
        {
            EnsureTypeSuitability(interfaceType);

            var assemblyName = new AssemblyName(AssemblyName);
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            var baseClass = typeof(object);

            var typeBuilder = moduleBuilder.DefineType(
                "AutoConfig.DynamicTypes._Implementation_Of_" + interfaceType.FullName.Replace('.', '_'),
                TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                baseClass,
                new[] { interfaceType });

            return typeBuilder
                .EmitConstructor(baseClass.GetConstructor(Type.EmptyTypes))
                .EmitProperties(interfaceType.GetProperties())
                .EmitProperties(interfaceType.GetInterfaces().SelectMany(i => i.GetProperties()))
                .CreateType();
        }

        static void EnsureTypeSuitability(Type type)
        {
            if (!type.IsVisible)
                throw new AutoConfigTypeGenerationException(
                    string.Format("Cannot generate an implementation of interface {0} because it is not externally accessible.", type));

            if (type.GetMethods().Any(method => !method.IsSpecialName)
                || type.GetInterfaces().SelectMany(i => i.GetMethods()).Any(method => !method.IsSpecialName))
                throw new AutoConfigTypeGenerationException(
                    string.Format("Cannot generate an implementation of interface {0} because it contains method definitions.", type));
        }
    }
}
