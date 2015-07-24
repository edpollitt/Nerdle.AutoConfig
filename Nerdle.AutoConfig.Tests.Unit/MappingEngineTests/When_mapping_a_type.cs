using System;
using System.Xml;
using System.Xml.Linq;
using FluentAssertions;
using Moq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mapping;
using Nerdle.AutoConfig.Strategy;
using Nerdle.AutoConfig.TypeGeneration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.MappingEngineTests
{
    class When_mapping_a_type
    {
        MappingEngine _engine;
        Mock<ISectionProvider> _sectionProvider;
        Mock<ITypeFactory> _typeFactory;
        Mock<IMappingFactory> _mappingFactory;
        Mock<IStrategyManager> _strategyManager;
        Mock<IMappingStrategy> _mappingStrategy;
        Mock<ITypeMapping> _typeMapping;
        TestableSection _section;
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
            _section = new TestableSection(_xElement);
            _fooInstance = new Foo();

            _strategyManager.Setup(sm => sm.GetStrategyFor<IFoo>()).Returns(_mappingStrategy.Object);
            _strategyManager.Setup(sm => sm.GetStrategyFor(typeof(IFoo))).Returns(_mappingStrategy.Object);
            _typeFactory.Setup(tf => tf.InstanceOf(typeof(IFoo))).Returns(_fooInstance);
            _mappingFactory.Setup(mf => mf.CreateMapping(typeof(Foo), _xElement, _mappingStrategy.Object))
                .Returns(_typeMapping.Object);
            
            _engine = new MappingEngine(_sectionProvider.Object, _typeFactory.Object, 
                _mappingFactory.Object, _strategyManager.Object);
        }

        [Test]
        public void The_section_name_is_provided_by_the_strategy()
        {
            _mappingStrategy.Setup(ms => ms.ConvertCase("Foo")).Returns("barbaz");
            _sectionProvider.Setup(sp => sp.GetSection("barbaz")).Returns(_section);
            _engine.Map<IFoo>();
        }

        [Test]
        public void The_section_name_can_be_optionally_overridden()
        {
            _mappingStrategy.Setup(ms => ms.ConvertCase("Foo")).Returns("barbaz");
            _sectionProvider.Setup(sp => sp.GetSection("qux")).Returns(_section);
            _engine.Map<IFoo>("qux");
        }

        [Test]
        public void An_exception_is_thrown_if_the_section_is_not_found()
        {
            _mappingStrategy.Setup(ms => ms.ConvertCase("Foo")).Returns("barbaz");
            
            Action mapping = () => _engine.Map<IFoo>();
            mapping.ShouldThrowExactly<AutoConfigMappingException>()
                .Where(e => e.Message.Contains("Could not load section 'barbaz'."));
        }

        [Test]
        public void A_mapping_is_created()
        {
            _mappingStrategy.Setup(ms => ms.ConvertCase("Foo")).Returns("barbaz");
            _sectionProvider.Setup(sp => sp.GetSection("barbaz")).Returns(_section);
            _engine.Map<IFoo>();
            _mappingFactory.Verify(mf => mf.CreateMapping(typeof(Foo), _xElement, _mappingStrategy.Object), Times.Once);
        }

        [Test]
        public void The_mapping_is_applied_to_the_generated_instance()
        {
            _mappingStrategy.Setup(ms => ms.ConvertCase("Foo")).Returns("barbaz");
            _sectionProvider.Setup(sp => sp.GetSection("barbaz")).Returns(_section);
            _engine.Map<IFoo>();
            _typeMapping.Verify(tm => tm.Apply(_fooInstance), Times.Once);
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

        class TestableSection : Section
        {
            public TestableSection(XElement xElement)
            {
                XElement = xElement;
            }

            protected override void DeserializeSection(XmlReader reader) {}
        }
    }
}
