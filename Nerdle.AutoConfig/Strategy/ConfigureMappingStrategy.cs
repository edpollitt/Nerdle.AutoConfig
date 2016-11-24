using System;
using System.Linq.Expressions;
using System.Reflection;
using Nerdle.AutoConfig.Casing;

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
            var propertyName = PropertyName(property);
            return PropertyStrategies.GetOrAdd(propertyName, _ => new ConfigurePropertyStrategy<TProperty>()) as IConfigurePropertyStrategy<TProperty>;
        }

        static string PropertyName<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            MemberExpression me = null;

            if (propertyExpression.Body is UnaryExpression) // required if boxing
            {
                me = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
            }

            else if (propertyExpression.Body is MemberExpression)
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