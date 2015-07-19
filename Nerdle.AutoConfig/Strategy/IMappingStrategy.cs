using System.Reflection;
namespace Nerdle.AutoConfig.Strategy
{
    interface IMappingStrategy
    {
        string ConvertCase(string s);

        string NameFor(PropertyInfo property);
    }
}