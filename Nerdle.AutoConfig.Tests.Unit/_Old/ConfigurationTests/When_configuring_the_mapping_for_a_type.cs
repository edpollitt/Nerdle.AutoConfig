//using System;
//using System.Collections;
//using System.Xml.Linq;
//using FluentAssertions;
//using Nerdle.AutoConfig.Casing;
//using Nerdle.AutoConfig.Configuration;
//using Nerdle.AutoConfig.Mappers;
//using NUnit.Framework;

//namespace Nerdle.AutoConfig.Tests.Unit.ConfigurationTests
//{
//    [TestFixture]
//    class When_configuring_the_mapping_for_a_type
//    {
//        [Test]
//        public void The_casing_can_be_set()
//        {
//            AutoConfig.WhenMapping<ICloneable>(mapping => { mapping.UseMatchingCase(); });
//            MappingConfigs.GetFor<ICloneable>().CaseConverter.Should().BeOfType<MatchingCaseConverter>();
            
//            AutoConfig.WhenMapping<ICloneable>(mapping => { mapping.UseCamelCase(); });
//            MappingConfigs.GetFor<ICloneable>().CaseConverter.Should().BeOfType<CamelCaseConverter>();
//        }

//        [Test]
//        public void The_casing_is_camelCase_by_default()
//        {
//            var config = new ConfigureMappingStrategy<ICloneable>();
//            config.CaseConverter.Should().BeOfType<CamelCaseConverter>();
//        }

//        [Test]
//        public void Property_mappings_can_be_added()
//        {
//            AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).From("itemCount"); });
//            var config = MappingConfigs.GetFor<IList>();
//            config.PropertyConfigs.Should().HaveCount(1);
//            config.PropertyConfigs.Keys.Should().Contain("Count");
//            config.PropertyConfigs["Count"].MapFrom.Should().Be("itemCount");
//        }

//        public void Properties_can_be_optional()
//        {
//            AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).Optional(); });
//            var config = MappingConfigs.GetFor<IList>();
//            config.PropertyConfigs.Should().HaveCount(1);
//            config.PropertyConfigs["Count"].IsOptional.Should().BeTrue();
//        }

//        public void Properties_can_have_default_values()
//        {
//            AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).OptionalWithDefault(42); });
//            var config = MappingConfigs.GetFor<IList>();
//            config.PropertyConfigs.Should().HaveCount(1);
//            config.PropertyConfigs["Count"].DefaultValue.Should().Be(42);
//        }
        
//        public void Properties_can_use_a_custom_mapper()
//        {
//            AutoConfig.WhenMapping<IList>(mapping => { mapping.Map(list => list.Count).Using<CustomerMapper>(); });
//            var config = MappingConfigs.GetFor<IList>();
//            config.PropertyConfigs.Should().HaveCount(1);
//            config.PropertyConfigs["Count"].Mapper.Should().NotBeNull();
//            config.PropertyConfigs["Count"].Mapper.Should().BeOfType<CustomerMapper>();
//        }

//        [Test]
//        public void A_mapping_can_be_configured_for_a_particular_type()
//        {
//            AutoConfig.WhenMapping<ICloneable>(mapping => { });
//            var config = MappingConfigs.GetFor<ICloneable>();
//            config.Should().NotBeNull();
//            config.Should().NotBe(MappingConfigs.Default);
//        }

//        [Test]
//        public void A_default_mapping_is_used_if_no_mapping_is_explicitly_configured()
//        {
//            var config = MappingConfigs.GetFor<ICloneable>();
//            config.Should().NotBeNull();
//            config.Should().Be(MappingConfigs.Default);
//        }

//        [Test]
//        public void Mappings_on_types_are_additive()
//        {
//            AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).From("itemCount"); });
//            AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.IsSynchronized).From("synced"); });
//            AutoConfig.WhenMapping<ICollection>(mapping => { mapping.UseMatchingCase(); });

//            var config = MappingConfigs.GetFor<ICollection>();
//            config.Should().NotBeNull(); 
//            config.PropertyConfigs.Should().HaveCount(2);
//            config.CaseConverter.Should().BeOfType<MatchingCaseConverter>();
//        }

//        [Test]
//        public void Mappings_on_individual_properties_are_additive()
//        {
//            AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).From("itemCount"); });
//            AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).OptionalWithDefault(1); });
//            AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).From("foo"); });
//            AutoConfig.WhenMapping<ICollection>(mapping => { mapping.Map(c => c.Count).From("bar"); });

//            var config = MappingConfigs.GetFor<ICollection>();
//            config.Should().NotBeNull();
//            config.PropertyConfigs.Should().HaveCount(1);
//            config.PropertyConfigs["Count"].MapFrom.Should().Be("bar");
//            config.PropertyConfigs["Count"].IsOptional.Should().BeTrue();
//            config.PropertyConfigs["Count"].DefaultValue.Should().Be(1);
//        }
//    }

//    class CustomerMapper : IMapper
//    {
//        public object Map(XElement element, Type type)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
