using System;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using Nerdle.AutoConfig.Mappers;
using Nerdle.AutoConfig.Mapping;
using NUnit.Framework;
using Moq;

namespace Nerdle.AutoConfig.Tests.Unit.Mapping.MappingFromElementTests
{
    [TestFixture]
    public class When_mapping_a_property
    {
        readonly PropertyInfo _propertyInfo = typeof(Foo).GetProperty("Bar");
        readonly XElement _xElement = new XElement("theElementName");
        Foo _instance;
        Mock<IMapper> _mapper;
        MappingFromElement _sut;

        [SetUp]
        public void BeforeEach()
        {
            _instance = new Foo();
            _mapper = new Mock<IMapper>();
            _sut = new MappingFromElement(_xElement, _propertyInfo, _mapper.Object);
        }

        [Test]
        public void The_property_is_set_from_the_mapper_return_value()
        {
            _mapper.Setup(m => m.Map(_xElement, _propertyInfo.PropertyType)).Returns("apples");
            _sut.Apply(_instance);
            _instance.Bar.Should().Be("apples");
        }

        [Test]
        public void A_mapping_exception_is_thrown_if_the_mapper_throws()
        {
            _mapper.Setup(m => m.Map(_xElement, _propertyInfo.PropertyType)).Throws<FormatException>();
            Action mapping = () => _sut.Apply(_instance);
            mapping.ShouldThrowExactly<AutoConfigMappingException>()
                .Where(m => m.Message.Contains("theElementName")
                            && m.Message.Contains("Bar")
                            && m.Message.Contains(typeof(Foo).Name)
                            && m.InnerException is FormatException);
        }
    }

    class Foo
    {
        public string Bar { get; set; }
    }
}
