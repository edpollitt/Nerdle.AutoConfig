using System;
using System.Xml.Linq;
using Nerdle.AutoConfig.Strategy;

namespace Nerdle.AutoConfig.Mapping
{
    interface IMappingFactory
    {
        TypeMapping CreateMapping(Type type, XElement element, IMappingStrategy mappingStrategy);
    }
}