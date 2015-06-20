using System;
using System.Linq;
using Nerdle.AutoConfig.Exceptions;

namespace Nerdle.AutoConfig.Mappers
{
    static class Mapper
    {
        static readonly IQueryableMapper[] Mappers = { new SimpletMapper(), new CollectionMapper() };
            
        public static IMapper For(Type type)
        {
            var mappers = Mappers.Where(m => m.CanMap(type)).ToList();

            if (mappers.Count == 0)
                throw new AutoConfigMappingException(
                    string.Format("No IMapper found to handle type '{0}'.", type));

            if (mappers.Count > 1)
                throw new AutoConfigMappingException(
                    string.Format("Multiple IMappers found to handle type '{0}'.", type));

            return mappers.Single();
        }
    }
}