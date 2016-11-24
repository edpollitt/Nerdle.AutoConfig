using Nerdle.AutoConfig.Mapping.Mappers;

namespace Nerdle.AutoConfig.Strategy
{
    class ConfigurePropertyStrategy<T> : PropertyStrategy, IConfigurePropertyStrategy<T>
    {
        public IConfigurePropertyStrategy<T> From(string elementOrAttributeName)
        {
            MapFrom = elementOrAttributeName;
            return this;
        }

        public IConfigurePropertyStrategy<T> Optional()
        {
            IsOptional = true;
            return this;
        }

        public IConfigurePropertyStrategy<T> OptionalWithDefault(T t)
        {
            Optional();
            DefaultValue = t;
            return this;
        }

        public IConfigurePropertyStrategy<T> Using<TMapper>() where TMapper : IMapper, new()
        {
            return Using(new TMapper());
        }

        public IConfigurePropertyStrategy<T> Using(IMapper mapper)
        {
            Mapper = mapper;
            return this;
        }
    }
}
