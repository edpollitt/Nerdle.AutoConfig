using System;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.PropertyMappingTests
{
    [TestFixture]
    public class When_mapping_a_property
    {
        readonly XElement _xElement = new XElement("testClass");
        readonly PropertyInfo _propertyInfo = typeof(TestClass).GetProperty("StringProperty");

        [Test]
        public void The_property_is_set_from_the_mapper_return_value()
        {
            var mapper = new TestMapper(() => "set");
            var propertyMapping = new PropertyMapping(_xElement, _propertyInfo, mapper);
            var instance = new TestClass();
            propertyMapping.Apply(instance);
            instance.StringProperty.Should().Be("set");
        }

        [Test]
        public void A_mapping_exception_is_thrown_if_the_mapper_throws()
        {
            var mapper = new TestMapper(() => { throw new FormatException(); });
            var propertyMapping = new PropertyMapping(_xElement, _propertyInfo, mapper);
            var instance = new TestClass();
            Action mapping = () => propertyMapping.Apply(instance);
            mapping.ShouldThrowExactly<AutoConfigMappingException>()
                .Where(m => m.Message.Contains(_xElement.Name.LocalName)
                         && m.Message.Contains(_propertyInfo.Name) 
                         && m.Message.Contains(typeof(TestClass).Name)
                         && m.InnerException is FormatException);
        }
    }

    class TestClass
    {
        public string StringProperty { get; set; }
    }

    class TestMapper : IMapper
    {
        readonly Func<string> _func;
    
        public TestMapper(Func<string> func)
        {
            _func = func;
        }

        public object Map(XElement element, Type type)
        {
            return _func();
        }
    }
}
