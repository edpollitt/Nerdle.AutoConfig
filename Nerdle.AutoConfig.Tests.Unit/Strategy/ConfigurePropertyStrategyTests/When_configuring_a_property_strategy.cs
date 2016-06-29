using System;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Strategy.ConfigurePropertyStrategyTests
{
    [TestFixture]
    class When_configuring_a_property_strategy
    {
        ConfigurePropertyStrategy<int> _sut;

        [SetUp]
        public void BeforeEach()
        {
            _sut = new ConfigurePropertyStrategy<int>();
        }

        [Test]
        public void The_property_can_be_set_as_optional()
        {
            _sut.Optional();
            _sut.IsOptional.Should().BeTrue();
        }

        [Test]
        public void A_default_value_can_be_set()
        {
            _sut.OptionalWithDefault(42);
            _sut.IsOptional.Should().BeTrue();
            _sut.DefaultValue.Should().Be(42);
        }

        [Test]
        public void The_xml_name_can_be_specified()
        {
            _sut.From("blah");
            _sut.MapFrom.Should().Be("blah");
        }

        [Test]
        public void A_custom_mapper_type_can_be_specified()
        {
            _sut.Using<CustomMapper>();
            _sut.Mapper.Should().NotBeNull();
            _sut.Mapper.Should().BeOfType<CustomMapper>();
        }

        [Test]
        public void A_custom_mapper_instance_can_be_supplied()
        {
            var customerMapper = new CustomMapper();
            _sut.Using(customerMapper);
            _sut.Mapper.Should().NotBeNull();
            _sut.Mapper.Should().Be(customerMapper);
        }
    }

    class CustomMapper : IMapper
    {
        public object Map(XElement element, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
