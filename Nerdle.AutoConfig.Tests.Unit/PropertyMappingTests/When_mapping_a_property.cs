using System;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mappers;
using Nerdle.AutoConfig.Mappings;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.PropertyMappingTests
{
    [TestFixture]
    public class When_mapping_a_property
    {
        [TestFixture]
        public class From_an_element
        {
            readonly PropertyInfo _propertyInfo = typeof(TestClass).GetProperty("IntProperty");
            readonly XElement _xElement = new XElement("testClass");

            [Test]
            public void The_property_is_set_from_the_mapper_return_value()
            {
                var mapper = new TestMapper(() => 42);
                var propertyMapping = new ElementMapping(_xElement, _propertyInfo, mapper);
                var instance = new TestClass();
                propertyMapping.Apply(instance);
                instance.IntProperty.Should().Be(42);
            }

            [Test]
            public void A_mapping_exception_is_thrown_if_the_mapper_throws()
            {
                var mapper = new TestMapper(() => { throw new FormatException(); });
                var propertyMapping = new ElementMapping(_xElement, _propertyInfo, mapper);
                var instance = new TestClass();
                Action mapping = () => propertyMapping.Apply(instance);
                mapping.ShouldThrowExactly<AutoConfigMappingException>()
                    .Where(m => m.Message.Contains(_xElement.Name.LocalName)
                                && m.Message.Contains(_propertyInfo.Name)
                                && m.Message.Contains(typeof (TestClass).Name)
                                && m.InnerException is FormatException);
            }
        }

        [TestFixture]
        public class From_an_attribute
        {
            readonly PropertyInfo _propertyInfo = typeof(TestClass).GetProperty("IntProperty");

            [Test]
            public void The_property_is_set_using_an_internal_conversion()
            {
                var xAttribute = new XAttribute("StringProperty", "42");
                var propertyMapping = new AttributeMapping(xAttribute, _propertyInfo);
                var instance = new TestClass();
                propertyMapping.Apply(instance);
                instance.IntProperty.Should().Be(42);
            }

            [Test]
            public void A_mapping_exception_is_thrown_if_the_conversion_throws()
            {
                var xAttribute = new XAttribute("IntProperty", "foo");
                var propertyMapping = new AttributeMapping(xAttribute, _propertyInfo);
                var instance = new TestClass();
                Action mapping = () => propertyMapping.Apply(instance);
                mapping.ShouldThrowExactly<AutoConfigMappingException>()
                    .Where(m => m.Message.Contains(xAttribute.Name.LocalName)
                                && m.Message.Contains(xAttribute.Value)
                                && m.Message.Contains(_propertyInfo.Name)
                                && m.Message.Contains(typeof(TestClass).Name)
                                && m.InnerException.Message == "foo is not a valid value for Int32.");
            }
        }
    }

    class TestClass
    {
        public int IntProperty { get; set; }
    }

    class TestMapper : IMapper
    {
        readonly Func<int> _func;
    
        public TestMapper(Func<int> func)
        {
            _func = func;
        }

        public object Map(XElement element, Type type)
        {
            return _func();
        }
    }
}
