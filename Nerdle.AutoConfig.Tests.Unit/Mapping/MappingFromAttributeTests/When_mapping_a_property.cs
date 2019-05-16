using System;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mapping;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mapping.MappingFromAttributeTests
{
    [TestFixture]
    public class When_mapping_a_property
    {
        [Test]
        public void The_property_is_set_using_an_internal_conversion()
        {
            var xAttribute = new XAttribute("whatever", "42");
            var propertyInfo = typeof(Foo).GetProperty(nameof(Foo.AnInt));
            var sut = new MappingFromAttribute(xAttribute, propertyInfo);
            var instance = new Foo();
            sut.Apply(instance);
            instance.AnInt.Should().Be(42);
        }

        [Test]
        public void A_mapping_exception_is_thrown_if_the_conversion_throws()
        {
            var xAttribute = new XAttribute("theAttributeName", "theInvalidValue");
            var propertyInfo = typeof(Foo).GetProperty(nameof(Foo.AnInt));
            var sut = new MappingFromAttribute(xAttribute, propertyInfo);
            var instance = new Foo();
            Action mapping = () => sut.Apply(instance);
            var exception = mapping.Should().ThrowExactly<AutoConfigMappingException>().Which;
            exception.Message.Should().Contain("theAttributeName")
                .And.Subject.Should().Contain("theInvalidValue")
                .And.Subject.Should().Contain(propertyInfo.Name)
                .And.Subject.Should().Contain(typeof(Foo).Name);
            exception.InnerException.Message.Should().Contain("theInvalidValue is not a valid value for Int32.");
        }
        
        [Test]
        public void A_mapping_exception_is_thrown_if_an_enum_is_undefined()
        {
            var xAttribute = new XAttribute("theAttributeName", "1234");
            var propertyInfo = typeof(Foo).GetProperty(nameof(Foo.AnEnum));
            var sut = new MappingFromAttribute(xAttribute, propertyInfo);
            var instance = new Foo();
            Action mapping = () => sut.Apply(instance);
            var exception = mapping.Should().ThrowExactly<AutoConfigMappingException>().Which;
            exception.Message.Should().Contain("theAttributeName")
                .And.Subject.Should().Contain("1234")
                .And.Subject.Should().Contain(propertyInfo.Name)
                .And.Subject.Should().Contain(typeof(Foo).Name);
            exception.InnerException.Should().BeOfType<ArgumentOutOfRangeException>();
        }
    }

    class Foo
    {
        public int AnInt { get; set; }
        public DayOfWeek AnEnum { get; set; }
    }
}
