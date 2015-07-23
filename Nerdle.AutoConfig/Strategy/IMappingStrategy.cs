using System.Reflection;

namespace Nerdle.AutoConfig.Strategy
{
    interface IMappingStrategy
    {
        string ConvertCase(string s);

        PropertyStrategy ForProperty(PropertyInfo property);
    }
}