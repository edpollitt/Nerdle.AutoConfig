using System.Reflection;

namespace Nerdle.AutoConfig.Strategy
{
    interface IMappingStrategy
    {
        string ConvertCase(string s);

        string SectionNameFor<T>();

        IPropertyStrategy ForProperty(PropertyInfo property);
    }
}