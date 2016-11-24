using System;
using System.Xml.Linq;
using Nerdle.AutoConfig.Mapping;
using Nerdle.AutoConfig.Sections;
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
            var strategy = _strategyManager.GetStrategyFor<T>();
            var section = string.IsNullOrWhiteSpace(sectionName) 
                ? _sectionProvider.GetSection<T>(strategy) : _sectionProvider.GetSection<T>(sectionName);
            return (T)Map(typeof(T), section, strategy);
        }

        public object Map(Type type, XElement element, IMappingStrategy strategy = null)
        {
            var instance = _typeFactory.InstanceOf(type);
            // since the type param might be an interface, and we need the concrete type
            var concreteType = instance.GetType();
            strategy = strategy ?? _strategyManager.GetStrategyFor(type); 
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