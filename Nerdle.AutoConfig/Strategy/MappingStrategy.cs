using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using Nerdle.AutoConfig.Casing;
using System.Reflection;
using Nerdle.AutoConfig.Extensions;

namespace Nerdle.AutoConfig.Strategy
{
    class MappingStrategy : IMappingStrategy
    {
        protected ConcurrentDictionary<string, PropertyStrategy> PropertyStrategies { get; private set; }

        public static readonly IPropertyStrategy DefaultPropertyStrategy = new PropertyStrategy();

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
            if (PropertyStrategies.TryGetValue(KeyFor(property), out var strategy))
                return strategy;

            var defaultValueAttribute = property.GetCustomAttributes<DefaultValueAttribute>(true).SingleOrDefault();
            if (defaultValueAttribute == null)
            {
                var interfaceProperties = property.DeclaringType?.GetInterfaces().Select(e => e.GetProperties().SingleOrDefault(p => p.Name == property.Name));
                defaultValueAttribute = interfaceProperties?.FirstOrDefault()?.GetCustomAttributes<DefaultValueAttribute>(true).SingleOrDefault();
            }
            return defaultValueAttribute != null ? new PropertyStrategy(defaultValueAttribute.Value) : DefaultPropertyStrategy;
        }

        static string KeyFor(PropertyInfo property)
        {
            return property.Name;
        }
    }
}