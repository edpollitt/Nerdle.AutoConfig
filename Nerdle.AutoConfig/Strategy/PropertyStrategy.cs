using Nerdle.AutoConfig.Mappers;

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
        public string MapFrom { get; protected set; }
        public bool IsOptional { get; protected set; }
        public object DefaultValue { get; protected set; }
        public IMapper Mapper { get; protected set; }
    }

    class NullablePropertyStrategy : PropertyStrategy
    {
        public NullablePropertyStrategy()
        {
            base.IsOptional = true;
        }
    }
}