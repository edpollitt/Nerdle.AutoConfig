using System;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Moq;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mappers;
using Nerdle.AutoConfig.Mapping;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mapping.MappingFactoryTests
{
    [TestFixture]
    public class When_creating_a_mapping
    {
        readonly MappingFactory _sut = new MappingFactory();
        Mock<IMappingStrategy> _strategy;
        Foo _foo;

        [SetUp]
        public void BeforeEach()
        {
            _strategy = new Mock<IMappingStrategy>();
            _strategy.Setup(s => s.ConvertCase(It.IsAny<string>())).Returns<string>(s => s.ToLowerInvariant());
            _strategy.Setup(s => s.ForProperty(It.IsAny<PropertyInfo>())).Returns(MappingStrategy.DefaultPropertyStrategy);
            _foo = new Foo();
        }

        [Test]
        public void A_mapping_is_created_if_properties_match_elements_and_attributes()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><baz>2</baz></foo>");
            var mapping = _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            
            mapping.Should().NotBeNull();
        }

        [Test]
        public void A_mapping_is_created_for_nullable_properties_when_no_element_is_present()
        {
            var xElement = XElement.Parse("<foo></foo>");
            _strategy.Setup(s => s.ForProperty(It.IsAny<PropertyInfo>())).Returns(MappingStrategy.DefaultNullablePropertyStrategy);
            var mapping = _mappingFactory.CreateMapping(typeof(FooWithNullableInt), xElement, _strategy.Object);

            mapping.Should().NotBeNull();
        }

        [Test]
        public void A_mapping_is_created_for_nullable_properties_when_element_is_present()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar></foo>");
            _strategy.Setup(s => s.ForProperty(It.IsAny<PropertyInfo>())).Returns(MappingStrategy.DefaultNullablePropertyStrategy);
            var mapping = _mappingFactory.CreateMapping(typeof(FooWithNullableInt), xElement, _strategy.Object);

            mapping.Should().NotBeNull();
        }

        [Test]
        public void A_mapping_for_nullable_properties_has_the_value_set()
        {
            var nullableFoo = new FooWithNullableInt();
            var xElement = XElement.Parse("<foo><bar>1</bar></foo>");
            _strategy.Setup(s => s.ForProperty(It.IsAny<PropertyInfo>())).Returns(MappingStrategy.DefaultNullablePropertyStrategy);
            _mappingFactory.CreateMapping(typeof(FooWithNullableInt), xElement, _strategy.Object).Apply(nullableFoo);

            nullableFoo.Bar.Should().Be(1);
        }

        [Test]
        public void Only_settable_public_properties_are_matched()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><baz>2</baz></foo>");
            var mapping = _sut.CreateMapping(typeof(FooWithSomeNonPublicStuff), xElement, _strategy.Object);
            
            mapping.Should().NotBeNull();
        }

        [Test]
        public void The_mapping_includes_all_the_properties()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><baz>2</baz></foo>");
            _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object).Apply(_foo);

            _foo.Bar.Should().Be(1);
            _foo.Baz.Should().Be(2);
        }

        [Test]
        public void An_exception_is_thrown_if_a_property_is_not_matched()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><qux>2</qux></foo>");
            Action creatingTheMapping = () => _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .Where(e => e.Message.Contains("Could not map property 'Baz'"));
        }

         [Test]
        public void Matching_is_case_sensitive()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><BAZ>2</BAZ></foo>");
            Action creatingTheMapping = () => _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            creatingTheMapping.ShouldThrowExactly<AutoConfigMappingException>()
                .Where(e => e.Message.Contains("Could not map property 'Baz'"));
        }

        [Test]
        public void Properties_can_be_matched_from_either_attributes_or_elements()
        {
            var xElement = XElement.Parse("<foo bar=\"1\"><baz>2</baz></foo>");
            _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object).Apply(_foo);

            _foo.Bar.Should().Be(1);
            _foo.Baz.Should().Be(2);
        }

        [Test]
        public void An_exception_is_thrown_if_an_element_is_not_matched()
        {
            var xElement = XElement.Parse("<foo><bar>1</bar><baz>2</baz><qux>1</qux></foo>");
            Action creatingTheMapping = () => _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .Where(e => e.Message.Contains("No matching settable property for config element 'qux' was found."));
        }

        [Test]
        public void An_exception_is_thrown_if_an_attribute_is_not_matched()
        {
            var xElement = XElement.Parse("<foo bar=\"1\" baz=\"1\" qux=\"1\" />");
            Action creatingTheMapping = () => _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .Where(e => e.Message.Contains("No matching settable property for config attribute 'qux' was found."));
        }

        [Test]
        public void The_casing_convention_is_provided_by_the_strategy()
        {
            var xElement = XElement.Parse("<foo><BAR>1</BAR><BAZ>2</BAZ></foo>");
            _strategy.Setup(s => s.ConvertCase(It.IsAny<string>())).Returns<string>(s => s.ToUpperInvariant());

            _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object).Apply(_foo);

            _foo.Bar.Should().Be(1);
            _foo.Baz.Should().Be(2);
        }

        [Test]
        public void A_property_name_can_be_overridden_by_the_strategy()
        {
            var propertyStrategy = new Mock<IPropertyStrategy>();
            propertyStrategy.Setup(s => s.MapFrom).Returns("bazbazbaz");
            _strategy.Setup(s => s.ForProperty(It.Is<PropertyInfo>(pi => pi.Name == "Baz"))).Returns(propertyStrategy.Object);

            var xElement = XElement.Parse("<foo><bar>1</bar><bazbazbaz>2</bazbazbaz></foo>");

            _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object).Apply(_foo);
            
            _foo.Bar.Should().Be(1);
            _foo.Baz.Should().Be(2);
        }

        [Test]
        public void A_property_can_be_set_as_optional_by_the_strategy()
        {
            var propertyStrategy = new Mock<IPropertyStrategy>();
            propertyStrategy.Setup(s => s.IsOptional).Returns(true);
            _strategy.Setup(s => s.ForProperty(It.Is<PropertyInfo>(pi => pi.Name == "Baz"))).Returns(propertyStrategy.Object);

            var xElement = XElement.Parse("<foo><bar>1</bar></foo>");

            _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object).Apply(_foo);

            _foo.Bar.Should().Be(1);
            _foo.Baz.Should().Be(0);
        }

        [Test]
        public void A_property_can_be_assigned_a_default_value_by_the_strategy()
        {
            var propertyStrategy = new Mock<IPropertyStrategy>();
            propertyStrategy.Setup(s => s.IsOptional).Returns(true);
            propertyStrategy.Setup(s => s.DefaultValue).Returns(42);
            _strategy.Setup(s => s.ForProperty(It.Is<PropertyInfo>(pi => pi.Name == "Baz"))).Returns(propertyStrategy.Object);

            var xElement = XElement.Parse("<foo><bar>1</bar></foo>");

            _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object).Apply(_foo);

            _foo.Bar.Should().Be(1);
            _foo.Baz.Should().Be(42);
        }

        [Test]
        public void A_custom_mapper_can_be_specified_by_the_strategy()
        {
            var propertyStrategy = new Mock<IPropertyStrategy>();
            var mapper = new Mock<IMapper>();
            propertyStrategy.Setup(s => s.Mapper).Returns(mapper.Object);
            mapper.Setup(m => m.Map(It.IsAny<XElement>(), It.IsAny<Type>())).Returns(123);
            _strategy.Setup(s => s.ForProperty(It.Is<PropertyInfo>(pi => pi.Name == "Baz"))).Returns(propertyStrategy.Object);

            var xElement = XElement.Parse("<foo><bar>1</bar><baz>2</baz></foo>");

            _sut.CreateMapping(typeof(Foo), xElement, _strategy.Object).Apply(_foo);

            _foo.Bar.Should().Be(1);
            _foo.Baz.Should().Be(123);
        }
    }

    class Foo
    {
        public int Bar { get; set; }
        public int Baz { get; set; }
    }

    class FooWithNullableInt
    {
        public int? Bar { get; set; }
    }

    class FooWithSomeNonPublicStuff : Foo
    {
        // should all be ignored
        public int NoPublicSetter { get; protected set; }
        public void AMethod() {}
        internal int Internal { get; set; }
        string Private { get; set; }
    }
}
