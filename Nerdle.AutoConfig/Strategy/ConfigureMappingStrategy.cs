using System;
using System.Linq.Expressions;
using System.Reflection;
using Nerdle.AutoConfig.Casing;
using Nerdle.AutoConfig.Mapping;

namespace Nerdle.AutoConfig.Strategy
{
    class ConfigureMappingStrategy<T> : MappingStrategy, IConfigureMappingStrategy<T>
    {
        public void UseMatchingCase()
        {
            CaseConverter = new MatchingCaseConverter();
        }

        public void UseCamelCase()
        {
            CaseConverter = new CamelCaseConverter();
        }

        public IConfigurePropertyStrategy<TProperty> Map<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var propertyName = GetPropertyName(property);
            //return PropertyConfigs.GetOrAdd(propertyName, new ConfigurePropertyMapping<TProperty>()) as IConfigurePropertyStrategy<TProperty>;
            return null;
        }

        static string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            MemberExpression me = null;

            // TODO this won't be needed unless we are boxing
            /*if (propertyExpression.Body is UnaryExpression)
            {
                me = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
            }

            else*/ if (propertyExpression.Body is MemberExpression)
            {
                me = (MemberExpression)propertyExpression.Body;
            }

            if (me == null)
            {
                throw new ArgumentException(string.Format("Could not derive MemberExpression from  type '{0}'.", propertyExpression.Body.GetType()));
            }

            var property = me.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("The MemberExpression is not a Property.");
            }

            return property.Name;
        }
    }
}