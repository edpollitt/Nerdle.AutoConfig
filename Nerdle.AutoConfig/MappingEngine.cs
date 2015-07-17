using System;
using System.Xml.Linq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Extensions;
using Nerdle.AutoConfig.Mapping;
using Nerdle.AutoConfig.Strategy;
using Nerdle.AutoConfig.TypeGeneration;

namespace Nerdle.AutoConfig
{
    class MappingEngine
    {
        readonly ISectionProvider _sectionProvider;
        readonly ITypeFactory _typeFactory;
        readonly IMappingFactory _mappingFactory;
        readonly IMappingStrategyProvider _strategyProvider;

        public MappingEngine(ISectionProvider sectionProvider, ITypeFactory typeFactory, IMappingFactory mappingFactory, IMappingStrategyProvider strategyProvider)
        {
            _sectionProvider = sectionProvider;
            _typeFactory = typeFactory;
            _mappingFactory = mappingFactory;
            _strategyProvider = strategyProvider;
        }

        public T Map<T>(string sectionName = null)
        {
            sectionName = sectionName ?? _strategyProvider.GetFor<T>().ConvertCase(typeof(T).SectionName());
            
            var section = _sectionProvider.GetSection(sectionName);

            if (section == null)
                throw new AutoConfigMappingException(
                    string.Format("Could not load section '{0}'. Make sure the section exists and is correctly cased.", sectionName));

            return (T)Map(typeof(T), section);
        }

        public object Map(Type type, XElement element)
        {
            var strategy = _strategyProvider.GetFor(type);
            var instance = _typeFactory.InstanceOf(type);
            // since the type param might be an interface, we need the actual type
            var concreteType = instance.GetType();
            var mapping = _mappingFactory.CreateFor(concreteType, element, strategy);
            mapping.Apply(instance);
            return instance;
        }

        public void WhenMapping<T>(Action<IConfigureMappingStrategy<T>> configureMapping)
        {
            //_strategyProvider.
            throw new NotImplementedException();
        }
    }
}