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
        ConfigurePropertyStrategy<int> _strategy;

        [SetUp]
        public void BeforeEach()
        {
            _strategy = new ConfigurePropertyStrategy<int>();
        }

        [Test]
        public void The_property_can_be_set_as_optional()
        {
            _strategy.Optional();
            _strategy.IsOptional.Should().BeTrue();
        }

        [Test]
        public void A_default_value_can_be_set()
        {
            _strategy.OptionalWithDefault(42);
            _strategy.IsOptional.Should().BeTrue();
            _strategy.DefaultValue.Should().Be(42);
        }

        [Test]
        public void The_xml_name_can_be_specified()
        {
            _strategy.From("blah");
            _strategy.MapFrom.Should().Be("blah");
        }

        [Test]
        public void A_customer_mapper_type_can_be_specified()
        {
            _strategy.Using<CustomerMapper>();
            _strategy.Mapper.Should().NotBeNull();
            _strategy.Mapper.Should().BeOfType<CustomerMapper>();
        }

        [Test]
        public void A_customer_mapper_instance_can_be_supplied()
        {
            var customerMapper = new CustomerMapper();
            _strategy.Using(customerMapper);
            _strategy.Mapper.Should().NotBeNull();
            _strategy.Mapper.Should().Be(customerMapper);
        }
    }

    class CustomerMapper : IMapper
    {
        public object Map(XElement element, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
