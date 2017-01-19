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
        MappingEngine _sut;
        Mock<ISectionProvider> _sectionProvider;
        Mock<ITypeFactory> _typeFactory;
        Mock<IMappingFactory> _mappingFactory;
        Mock<IStrategyManager> _strategyManager;
        Mock<IMappingStrategy> _mappingStrategy;
        Mock<ITypeMapping> _typeMapping;
        SectionStub _section;
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
            _section = new SectionStub(_xElement);
            _fooInstance = new Foo();

            _sectionProvider.Setup(sp => sp.GetSection<IFoo>(_mappingStrategy.Object, null)).Returns(_section);
            _strategyManager.Setup(sm => sm.GetStrategyFor<IFoo>()).Returns(_mappingStrategy.Object);
            _typeFactory.Setup(tf => tf.InstanceOf(typeof(IFoo))).Returns(_fooInstance);
            _mappingFactory.Setup(mf => mf.CreateMapping(typeof(Foo), _xElement, _mappingStrategy.Object))
                .Returns(_typeMapping.Object);
            
            _sut = new MappingEngine(_sectionProvider.Object, _typeFactory.Object, _mappingFactory.Object, _strategyManager.Object);
        }

        [Test]
        public void The_section_is_located()
        {
            _sut.Map<IFoo>();
            _sectionProvider.Verify(sp => sp.GetSection<IFoo>(_mappingStrategy.Object, null), Times.Once);
        }

        [Test]
        public void A_mapping_is_created()
        {
            _sut.Map<IFoo>();
            _mappingFactory.Verify(mf => mf.CreateMapping(typeof(Foo), _xElement, _mappingStrategy.Object), Times.Once);
        }

        [Test]
        public void An_instance_is_generated()
        {
            _sut.Map<IFoo>();
            _typeFactory.Verify(tf => tf.InstanceOf(typeof(IFoo)), Times.Once);
        }

        [Test]
        public void The_mapping_is_applied_to_the_generated_instance()
        {
            _sut.Map<IFoo>();
            _typeMapping.Verify(tm => tm.Apply(_fooInstance), Times.Once);
        }

        [Test]
        public void The_section_can_be_located_by_a_specified_name()
        {
            _sectionProvider.Setup(sp => sp.GetSection<IFoo>("foobar", null)).Returns(_section);
            _sut.Map<IFoo>("foobar");
            _sectionProvider.Verify(sp => sp.GetSection<IFoo>("foobar", null), Times.Once);
        }

        [Test]
        public void The_strategy_can_be_configured()
        {
            var action = new Action<IConfigureMappingStrategy<IFoo>>(foo => { });
            _sut.WhenMapping(action);
            _strategyManager.Verify(sm => sm.UpdateStrategy(action), Times.Once);
        }

        interface IFoo { }
        class Foo : IFoo { }

        class SectionStub : Section
        {
            public SectionStub(XElement xElement)
            {
                XElement = xElement;
            }

            protected override void DeserializeSection(XmlReader reader) {}
        }
    }
}
