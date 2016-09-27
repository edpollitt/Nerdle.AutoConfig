using FluentAssertions;
using Nerdle.AutoConfig.Casing;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Strategy.ConfigureMappingStrategyTests
{
    [TestFixture]
    class When_configuring_a_mapping_strategy
    {
        ConfigureMappingStrategy<IFoo> _sut;

        [SetUp]
        public void BeforeEach()
        {
            _sut = new ConfigureMappingStrategy<IFoo>();
        }

        [Test]
        public void Casing_can_be_set_to_matching_case()
        {
            _sut.UseMatchingCase();
            _sut.CaseConverter.Should().BeOfType<MatchingCaseConverter>();
        }

        [Test]
        public void Casing_can_be_set_to_camel_case()
        {
            _sut.UseCamelCase();
            _sut.CaseConverter.Should().BeOfType<CamelCaseConverter>();
        }

        [Test]
        public void The_default_casing_is_camel_case()
        {
            _sut.CaseConverter.Should().BeOfType<CamelCaseConverter>();
        }

        [Test]
        public void A_strategy_can_be_configured_for_a_specific_property()
        {
            _sut.Map(foo => foo.Bar).From("barbarbar");
            var bar = typeof(IFoo).GetProperty("Bar");
            _sut.ForProperty(bar).Should().NotBeNull();
            _sut.ForProperty(bar).MapFrom.Should().Be("barbarbar");
        }

        [Test]
        public void A_default_property_strategy_is_used_if_no_strategy_configured_for_the_specific_property()
        {
            var bar = typeof(IFoo).GetProperty("Bar");
            _sut.ForProperty(bar).Should().NotBeNull();
            _sut.ForProperty(bar).Should().Be(MappingStrategy.DefaultPropertyStrategy);
        }

        [Test]
        public void A_default_nullable_Property_strategy_is_used_if_no_strategy_configured_for_the_specific_property()
        {
            var bar = typeof(IFooWithNullableInt).GetProperty("Bar");
            _strategy.ForProperty(bar).Should().NotBeNull();
            _strategy.ForProperty(bar).Should().Be(MappingStrategy.DefaultNullablePropertyStrategy);
        }

        [Test]
        public void Strategy_configuration_is_additive()
        {
            _sut.Map(foo => foo.Bar).From("foo").OptionalWithDefault("one");
            _sut.Map(foo => foo.Bar).From("bar");
            _sut.Map(foo => foo.Bar).OptionalWithDefault("two");
            _sut.Map(foo => foo.Bar).From("baz").Optional();
            
            var bar = typeof(IFoo).GetProperty("Bar");
            _sut.ForProperty(bar).Should().NotBeNull();
            _sut.ForProperty(bar).MapFrom.Should().Be("baz");
            _sut.ForProperty(bar).DefaultValue.Should().Be("two");
            _sut.ForProperty(bar).IsOptional.Should().BeTrue();
        }
    }

    interface IFoo
    {
        string Bar { get; }
    }

    interface IFooWithNullableInt
    {
        int? Bar { get; }
    }
}
