using System;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using NUnit.Framework;
using Nerdle.AutoConfig.Mapping;
using Moq;
using Nerdle.AutoConfig.Strategy;
using System.Reflection;

namespace Nerdle.AutoConfig.Tests.Unit.Mapping.MappingFactoryTests
{
    [TestFixture]
    public class When_creating_a_mapping
    {
        readonly MappingFactory _mappingFactory = new MappingFactory();
        Mock<IMappingStrategy> _strategy;

        [SetUp]
        public void BeforeEach()
        {
            _strategy = new Mock<IMappingStrategy>();
            //_strategy.Setup(s => s.NameFor(It.IsAny<PropertyInfo>()))
            //    .Returns<PropertyInfo>(pi => pi.Name.ToLowerInvariant());
        }

        [Test]
        public void A_mapping_is_created_if_settable_properties_exactly_match_xml()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><baz>1</baz></foo>");
            var mapping = _mappingFactory.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            mapping.Should().NotBeNull();
        }

        [Test]
        public void An_exception_is_thrown_if_a_property_is_not_matched()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar></foo>");
            Action creatingTheMapping = () => _mappingFactory.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .WithMessage(string.Format("Could not map property 'Baz' for type '{0}' from section 'foo'. No matching config element or attribute was found.", typeof(Foo)));
        }

        [Test]
        public void Properties_can_be_matched_from_either_attributes_or_elements()
        {
            var xElement = XElement.Parse("<foo bar=\"1\"><baz>1</baz></foo>");
            var mapping = _mappingFactory.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            mapping.Should().NotBeNull();
        }

        [Test]
        public void An_exception_is_thrown_if_an_element_is_not_matched()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><baz>1</baz><qux>1</qux></foo>");
            Action creatingTheMapping = () => _mappingFactory.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .WithMessage(string.Format("Could not map type '{0}' from section 'foo'. No matching settable property for config element 'qux' was found.", typeof(Foo)));
        }

        [Test]
        public void An_exception_is_thrown_if_an_attribute_is_not_matched()
        {
            var xElement = XElement.Parse("<foo bar=\"1\" baz=\"1\" qux=\"1\" />");
            Action creatingTheMapping = () => _mappingFactory.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .WithMessage(string.Format("Could not map type '{0}' from section 'foo'. No matching settable property for config attribute 'qux' was found.", typeof(Foo)));
        }

        [Test]
        public void Names_are_provided_by_the_strategy()
        {
            //var xElement = XElement.Parse("<foo DOG=\"1\"><CAT>1</CAT></foo>");
            //_strategy.Setup(s => s.NameFor(It.Is<PropertyInfo>(p => p.Name == "Bar"))).Returns("DOG");
            //_strategy.Setup(s => s.NameFor(It.Is<PropertyInfo>(p => p.Name == "Baz"))).Returns("CAT");
            //var mapping = _mappingFactory.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            //mapping.Should().NotBeNull();
            Assert.Fail();
        }
    }

    class Foo
    {
        public int Bar { get; set; }
        public int Baz { get; set; }
        
        // should be ignored
        public int NoPublicSetter { get; private set; }
    }
}
