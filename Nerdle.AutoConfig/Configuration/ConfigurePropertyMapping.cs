using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig.Configuration
{
    class ConfigurePropertyMapping<T> : PropertyMappingConfig, IConfigurePropertyMapping<T>
    {
        public IConfigurePropertyMapping<T> From(string elementOrAttributeName)
        {
            MapFrom = elementOrAttributeName;
            return this;
        }

        public IConfigurePropertyMapping<T> Optional()
        {
            IsOptional = true;
            return this;
        }

        public IConfigurePropertyMapping<T> OptionalWithDefault(T t)
        {
            Optional();
            DefaultValue = t;
            return this;
        }

        public IConfigurePropertyMapping<T> Using<TMapper>() where TMapper : IMapper, new()
        {
            return Using(new TMapper());
        }

        public IConfigurePropertyMapping<T> Using(IMapper mapper)
        {
            Mapper = mapper;
            return this;
        }
    }
}