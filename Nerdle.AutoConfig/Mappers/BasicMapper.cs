using System;
using System.Reflection;
using System.Xml.Linq;

namespace Nerdle.AutoConfig.Mappers
{
    public class BasicMapper<T> : IMapper<T>
    {
        public void Map(XElement element, PropertyInfo property, T instance)
        {
            throw new NotImplementedException();
        }
    }
}