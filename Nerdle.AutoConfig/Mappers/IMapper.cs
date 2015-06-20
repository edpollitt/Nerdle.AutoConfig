using System.Reflection;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    interface IMapper
    {
        void Map(XElement element, PropertyInfo property, object instance);
    }
}