using System.Reflection;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    interface IMapper<in T>
    {
        void Map(XElement element, PropertyInfo property, T instance);
    }
}