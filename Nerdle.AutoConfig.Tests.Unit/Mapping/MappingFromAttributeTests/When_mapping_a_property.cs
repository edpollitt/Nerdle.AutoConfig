using System;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mappers;
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
            var propertyMapping = new MappingFromAttribute(xAttribute, _propertyInfo);
            var instance = new Foo();
            propertyMapping.Apply(instance);
            instance.Bar.Should().Be(42);
        }

        [Test]
        public void A_mapping_exception_is_thrown_if_the_conversion_throws()
        {
            var xAttribute = new XAttribute("theAttributeName", "theInvalidValue");
            var propertyMapping = new MappingFromAttribute(xAttribute, _propertyInfo);
            var instance = new Foo();
            Action mapping = () => propertyMapping.Apply(instance);
            mapping.ShouldThrowExactly<AutoConfigMappingException>()
                .Where(m => m.Message.Contains("theAttributeName")
                            && m.Message.Contains("theInvalidValue")
                            && m.Message.Contains(_propertyInfo.Name)
                            && m.Message.Contains(typeof(Foo).Name)
                            && m.InnerException.Message == "theInvalidValue is not a valid value for Int32.");
        }
    }

    class Foo
    {
        public int Bar { get; set; }
    }
}
