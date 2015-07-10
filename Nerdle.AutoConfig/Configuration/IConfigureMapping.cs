using System;
using System.Linq.Expressions;

namespace Nerdle.AutoConfig.Configuration
{
    public interface IConfigureMapping<T>
    {
        /// <summary>
        /// Configures the mapping to try to match property names exactly.
        /// </summary>
        void UseMatchingCase();

        /// <summary>
        /// Configures the mapping to try to match property names to camelCased element/attribute names.
        /// </summary>
        void UseCamelCase();

        /// <summary>
        /// Configures mapping for the specified property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IConfigurePropertyMapping<TProperty> Map<TProperty>(Expression<Func<T, TProperty>> property);
    }
}
