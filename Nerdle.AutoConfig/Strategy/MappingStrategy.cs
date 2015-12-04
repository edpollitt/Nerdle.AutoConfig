using System;
using System.Collections.Concurrent;
using Nerdle.AutoConfig.Casing;
using System.Reflection;
using Nerdle.AutoConfig.Extensions;

namespace Nerdle.AutoConfig.Strategy
{
    class MappingStrategy : IMappingStrategy
    {
        protected ConcurrentDictionary<string, PropertyStrategy> PropertyStrategies { get; private set; }

        public static readonly PropertyStrategy DefaultPropertyStrategy = new PropertyStrategy();
        public static readonly PropertyStrategy DefaultNullablePropertyStrategy = new NullablePropertyStrategy();

        public ICaseConverter CaseConverter { get; protected set; }

        public MappingStrategy()
        {
            PropertyStrategies = new ConcurrentDictionary<string, PropertyStrategy>();
            CaseConverter = new CamelCaseConverter();
        }

        public string ConvertCase(string s)
        {
            return CaseConverter.Convert(s);
        }

        public string SectionNameFor<T>()
        {
            return ConvertCase(typeof(T).ConcreteName());
        }

        public IPropertyStrategy ForProperty(PropertyInfo property)
        {
            PropertyStrategy strategy;
            return PropertyStrategies.TryGetValue(KeyFor(property), out strategy)
                ? strategy
                : Nullable.GetUnderlyingType(property.PropertyType) != null
                    ? DefaultNullablePropertyStrategy
                    : DefaultPropertyStrategy;
        }

        static string KeyFor(PropertyInfo property)
        {
            return property.Name;
        }
    }
}