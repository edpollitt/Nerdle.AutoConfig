using System;
using System.Linq;
using Nerdle.AutoConfig.Exceptions;

namespace Nerdle.AutoConfig.Mapping.Mappers
{
    public static class MapperSelector
    {
        static readonly ISelectableMapper[] SelectableMappers =
            {
                new ValueMapper(),
                new CollectionMapper(),
                new ArrayMapper(),
                new KeyValuePairMapper(),
                new DictionaryMapper()
            };

        public static IMapper GetFor(Type type)
        {
            var mappers = SelectableMappers.Where(m => m.CanMap(type)).ToList();

            if (mappers.Count > 1)
                throw new AutoConfigMappingException(
                    string.Format("Multiple IMappers found to handle type '{0}'.", type));

            return mappers.Count == 1 ? (IMapper)mappers.Single() : new RecursingMapper();
        }
    }
}