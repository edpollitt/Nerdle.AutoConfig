using Nerdle.AutoConfig.Mapping.Mappers;

namespace Nerdle.AutoConfig.Strategy
{
    interface IPropertyStrategy
    {
        string MapFrom { get; }
        bool IsOptional { get; }
        object DefaultValue { get; }
        IMapper Mapper { get; }
    }

    class PropertyStrategy : IPropertyStrategy
    {
        public PropertyStrategy()
        {
        }

        public PropertyStrategy(object defaultValue)
        {
            IsOptional = true;
            DefaultValue = defaultValue;
        }

        public string MapFrom { get; protected set; }
        public bool IsOptional { get; protected set; }
        public object DefaultValue { get; protected set; }
        public IMapper Mapper { get; protected set; }
    }
}