using System;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Casing;
using Nerdle.AutoConfig.Mappers;
using Nerdle.AutoConfig.Strategy;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Strategy.ConfigureMappingStrategyTests
{
    [TestFixture]
    class When_configuring_a_mapping_strategy
    {
        [Test]
        public void Casing_can_be_set()
        {
            var strategy = new ConfigureMappingStrategy<IFoo>();
            strategy.UseMatchingCase();
            strategy.CaseConverter.Should().BeOfType<MatchingCaseConverter>();
            strategy.UseCamelCase();
            strategy.CaseConverter.Should().BeOfType<CamelCaseConverter>();
        }

        [Test]
        public void The_default_casing_is_camelCase()
        {
            var strategy = new ConfigureMappingStrategy<IFoo>();
            strategy.CaseConverter.Should().BeOfType<CamelCaseConverter>();
        }


        //[Test]
        //public void Property_mappings_can_be_added()
        //{
        //    AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).From("itemCount"); });
        //    var config = MappingConfigs.GetStrategyFor<IList>();
        //    config.PropertyConfigs.Should().HaveCount(1);
        //    config.PropertyConfigs.Keys.Should().Contain("Count");
        //    config.PropertyConfigs["Count"].MapFrom.Should().Be("itemCount");
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

    class CustomerMapper : IMapper
    {
        public object Map(XElement element, Type type)
        {
            throw new NotImplementedException();
        }
    }

    interface IFoo
    {
            
    }
}
