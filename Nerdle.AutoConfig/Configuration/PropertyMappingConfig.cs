using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig.Configuration
{
    class PropertyMappingConfig
    {
        public string MapFrom { get; protected set; }
        public bool IsOptional { get; protected set; }
        public object DefaultValue { get; protected set; }
        public IMapper Mapper { get; protected set; }
    }
}