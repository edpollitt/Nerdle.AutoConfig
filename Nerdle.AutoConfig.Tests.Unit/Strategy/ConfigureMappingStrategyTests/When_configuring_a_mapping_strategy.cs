using FluentAssertions;
using Nerdle.AutoConfig.Casing;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Strategy.ConfigureMappingStrategyTests
{
    [TestFixture]
    class When_configuring_a_mapping_strategy
    {
        ConfigureMappingStrategy<IFoo> _strategy;

        [SetUp]
        public void BeforeEach()
        {
            _strategy = new ConfigureMappingStrategy<IFoo>();
        }

        [Test]
        public void Casing_can_be_set()
        {
            _strategy.UseMatchingCase();
            _strategy.CaseConverter.Should().BeOfType<MatchingCaseConverter>();
            _strategy.UseCamelCase();
            _strategy.CaseConverter.Should().BeOfType<CamelCaseConverter>();
        }

        [Test]
        public void The_default_casing_is_camelCase()
        {
            _strategy.CaseConverter.Should().BeOfType<CamelCaseConverter>();
        }

        [Test]
        public void A_strategy_can_be_configured_for_a_specific_property()
        {
            _strategy.Map(foo => foo.Bar).From("barbarbar");
            var bar = typeof(IFoo).GetProperty("Bar");
            _strategy.ForProperty(bar).Should().NotBeNull();
            _strategy.ForProperty(bar).MapFrom.Should().Be("barbarbar");
        }

        [Test]
        public void A_default_property_strategy_is_used_if_no_strategy_configured_for_the_specific_property()
        {
            var bar = typeof(IFoo).GetProperty("Bar");
            _strategy.ForProperty(bar).Should().NotBeNull();
            _strategy.ForProperty(bar).Should().Be(MappingStrategy.DefaultPropertyStrategy);
        }


        //[Test]
        //public void From_name_can_be_configured_for_a_property()
        //{
        //    var strategy = new ConfigureMappingStrategy<IFoo>();
        //    strategy.Map(foo => foo.Bar).From("barbarbar");

        //    var bar = typeof(IFoo).GetProperty("Bar");
        //    strategy.ForProperty(bar).Should().NotBeNull();
        //    //strategy.NameFor(bar).Should().Be("barbarbar");
        //    Assert.Fail();
        //}

        //public void Properties_can_be_optional()
        //{
        //    AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).Optional(); });
        //    var config = MappingConfigs.GetStrategyFor<IList>();
        //    config.PropertyConfigs.Should().HaveCount(1);
        //    config.PropertyConfigs["Count"].IsOptional.Should().BeTrue();
        //}

        //public void Properties_can_have_default_values()
        //{
        //    AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).OptionalWithDefault(42); });
        //    var config = MappingConfigs.GetStrategyFor<IList>();
        //    config.PropertyConfigs.Should().HaveCount(1);
        //    config.PropertyConfigs["Count"].DefaultValue.Should().Be(42);
        //}

        //public void Properties_can_use_a_custom_mapper()
        //{
        //    AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).Using<CustomerMapper>(); });
        //    var config = MappingConfigs.GetStrategyFor<IList>();
        //    config.PropertyConfigs.Should().HaveCount(1);
        //    config.PropertyConfigs["Count"].Mapper.Should().NotBeNull();
        //    config.PropertyConfigs["Count"].Mapper.Should().BeOfType<CustomerMapper>();
        //}

        //[Test]
        //public void Mappings_on_individual_properties_are_additive()
        //{
        //    AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).From("itemCount"); });
        //    AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).OptionalWithDefault(1); });
        //    AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).From("foo"); });
        //    AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).From("bar"); });

        //    var config = MappingConfigs.GetStrategyFor<ICollection>();
        //    config.Should().NotBeNull();
        //    config.PropertyConfigs.Should().HaveCount(1);
        //    config.PropertyConfigs["Count"].MapFrom.Should().Be("bar");
        //    config.PropertyConfigs["Count"].IsOptional.Should().BeTrue();
        //    config.PropertyConfigs["Count"].DefaultValue.Should().Be(1);
        //}
    }

    interface IFoo
    {
        string Bar { get; }
    }
}
