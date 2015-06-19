using System;
using System.Reflection;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mappers;

namespace Nerdle.AutoConfig
{
    class PropertyMapping<T>
    {
        readonly XElement _element;
        readonly PropertyInfo _property;
        readonly IMapper<T> _mapper;

        public PropertyMapping(XElement element, PropertyInfo property, IMapper<T> mapper)
        {
            _element = element;
            _property = property;
            _mapper = mapper;
        }

        public void Apply(T instance)
        {
            try
            {
                _mapper.Map(_element, _property, instance);
            }
            catch (Exception ex)
            {
                var message =
                    string.Format(
                        "Tried to map element {0} to property {1} of {2} using {3}, and barfed. See inner exception for details.",
                        _element.Name,
                        _property.Name,
                        typeof(T),
                        _mapper.GetType().Name);

                throw new AutoConfigMappingException(message, ex);
            }
        }
    }
}