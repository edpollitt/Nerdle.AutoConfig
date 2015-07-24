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
        readonly IStrategyManager _strategyManager;

        public MappingEngine(ISectionProvider sectionProvider, ITypeFactory typeFactory, IMappingFactory mappingFactory, IStrategyManager strategyManager)
        {
            _sectionProvider = sectionProvider;
            _typeFactory = typeFactory;
            _mappingFactory = mappingFactory;
            _strategyManager = strategyManager;
        }

        public T Map<T>(string sectionName = null)
        {
            sectionName = sectionName ?? _strategyManager.GetStrategyFor<T>().ConvertCase(typeof(T).ConcreteName());
            
            var section = _sectionProvider.GetSection(sectionName);

            if (section == null)
                throw new AutoConfigMappingException(
                    string.Format("Could not load section '{0}'. Make sure the section exists and is correctly cased.", sectionName));

            return (T)Map(typeof(T), section);
        }

        public object Map(Type type, XElement element)
        {
            var strategy = _strategyManager.GetStrategyFor(type);
            var instance = _typeFactory.InstanceOf(type);
            // since the type param might be an interface, we need the actual type
            var concreteType = instance.GetType();
            var mapping = _mappingFactory.CreateMapping(concreteType, element, strategy);
            mapping.Apply(instance);
            return instance;
        }

        public void WhenMapping<T>(Action<IConfigureMappingStrategy<T>> configureMappingStrategy)
        {
            _strategyManager.UpdateStrategy(configureMappingStrategy);
        }
    }
}