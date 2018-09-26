using System;
using System.Reflection;
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
        readonly PropertyInfo _propertyInfo = typeof(Foo).GetProperty("Bar");

        [Test]
        public void The_property_is_set_using_an_internal_conversion()
        {
            var xAttribute = new XAttribute("whatever", "42");
            var sut = new MappingFromAttribute(xAttribute, _propertyInfo);
            var instance = new Foo();
            sut.Apply(instance);
            instance.Bar.Should().Be(42);
        }

        [Test]
        public void A_mapping_exception_is_thrown_if_the_conversion_throws()
        {
            var xAttribute = new XAttribute("theAttributeName", "theInvalidValue");
            var sut = new MappingFromAttribute(xAttribute, _propertyInfo);
            var instance = new Foo();
            Action mapping = () => sut.Apply(instance);
            var exception = mapping.Should().ThrowExactly<AutoConfigMappingException>().Which;
            exception.Message.Should().Contain("theAttributeName")
                .And.Subject.Should().Contain("theInvalidValue")
                .And.Subject.Should().Contain(_propertyInfo.Name)
                .And.Subject.Should().Contain(typeof(Foo).Name);
            exception.InnerException.Message.Should().Contain("theInvalidValue is not a valid value for Int32.");
        }
    }

    class Foo
    {
        public int Bar { get; set; }
    }
}
