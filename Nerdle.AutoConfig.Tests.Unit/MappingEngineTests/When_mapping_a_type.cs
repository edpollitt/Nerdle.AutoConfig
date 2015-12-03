using System;
using System.Xml;
using System.Xml.Linq;
using Moq;
using Nerdle.AutoConfig.Mapping;
using Nerdle.AutoConfig.Sections;
using Nerdle.AutoConfig.Strategy;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.MappingEngineTests
{
    [TestFixture]
    class When_mapping_a_type
    {
        MappingEngine _engine;
        Mock<ISectionProvider> _sectionProvider;
        Mock<ITypeFactory> _typeFactory;
        Mock<IMappingFactory> _mappingFactory;
        Mock<IStrategyManager> _strategyManager;
        Mock<IMappingStrategy> _mappingStrategy;
        Mock<ITypeMapping> _typeMapping;
        DummySection _section;
        Foo _fooInstance;
        XElement _xElement;
        
        [SetUp]
        public void BeforeEach()
        {
            _sectionProvider =new Mock<ISectionProvider>();
            _typeFactory = new Mock<ITypeFactory>();
            _mappingFactory = new Mock<IMappingFactory>();
            _strategyManager = new Mock<IStrategyManager>();
            _mappingStrategy = new Mock<IMappingStrategy>();
            _typeMapping = new Mock<ITypeMapping>();

            _xElement = new XElement("element");
            _section = new DummySection(_xElement);
            _fooInstance = new Foo();

            _sectionProvider.Setup(sp => sp.GetSection<IFoo>(_mappingStrategy.Object)).Returns(_section);
            _strategyManager.Setup(sm => sm.GetStrategyFor<IFoo>()).Returns(_mappingStrategy.Object);
            _typeFactory.Setup(tf => tf.InstanceOf(typeof(IFoo))).Returns(_fooInstance);
            _mappingFactory.Setup(mf => mf.CreateMapping(typeof(Foo), _xElement, _mappingStrategy.Object))
                .Returns(_typeMapping.Object);
            
            _engine = new MappingEngine(_sectionProvider.Object, _typeFactory.Object, _mappingFactory.Object, _strategyManager.Object);
        }

        [Test]
        public void The_section_is_located()
        {
            _engine.Map<IFoo>();
            _sectionProvider.Verify(sp => sp.GetSection<IFoo>(_mappingStrategy.Object), Times.Once);
        }

        [Test]
        public void A_mapping_is_created()
        {
            _engine.Map<IFoo>();
            _mappingFactory.Verify(mf => mf.CreateMapping(typeof(Foo), _xElement, _mappingStrategy.Object), Times.Once);
        }

        [Test]
        public void An_instance_is_generated()
        {
            _engine.Map<IFoo>();
            _typeFactory.Verify(tf => tf.InstanceOf(typeof(IFoo)), Times.Once);
        }

        [Test]
        public void The_mapping_is_applied_to_the_generated_instance()
        {
            _engine.Map<IFoo>();
            _typeMapping.Verify(tm => tm.Apply(_fooInstance), Times.Once);
        }

        [Test]
        public void The_section_can_be_located_by_a_specified_name()
        {
            _sectionProvider.Setup(sp => sp.GetSection<IFoo>("foobar")).Returns(_section);
            _engine.Map<IFoo>("foobar");
            _sectionProvider.Verify(sp => sp.GetSection<IFoo>("foobar"), Times.Once);
        }

        [Test]
        public void The_strategy_can_be_configured()
        {
            var action = new Action<IConfigureMappingStrategy<IFoo>>(foo => { });
            _engine.WhenMapping(action);
            _strategyManager.Verify(sm => sm.UpdateStrategy(action), Times.Once);
        }

        interface IFoo { }
        class Foo : IFoo { }

        class DummySection : Section
        {
            public DummySection(XElement xElement)
            {
                XElement = xElement;
            }

            protected override void DeserializeSection(XmlReader reader) {}
        }
    }
}
