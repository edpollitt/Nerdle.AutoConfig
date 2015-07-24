using System.Reflection;

namespace Nerdle.AutoConfig.Strategy
{
    interface IMappingStrategy
    {
        string ConvertCase(string s);

        IPropertyStrategy ForProperty(PropertyInfo property);
    }
}