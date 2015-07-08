using System;
using FluentAssertions;
using Nerdle.AutoConfig.CaseConverters;
using Nerdle.AutoConfig.Configuration;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.ConfigurationTests
{
    [TestFixture]
    class When_configuring_a_mapping
    {
        [Test]
        public void A_case_converter_can_be_set()
        {
            var mappingConfig = new MappingConfig(new CaseCoverterWhichReturns("foobar"));
            mappingConfig.ConvertCase("anything").Should().Be("foobar");
        }

        // TODO: these tests don't really do anything yet

        [Test]
        public void A_mapping_can_be_configured_for_a_particular_type()
        {
            AutoConfig.WhenMapping<ICloneable>(mapping => mapping.UseMatchingCase());
            var config = MappingConfigs.Get<ICloneable>();
            config.Should().NotBeNull();
        }

        [Test]
        public void A_default_mapping_is_used_if_no_mapping_is_explicitly_configured()
        {
            AutoConfig.WhenMapping<ICloneable>(mapping => mapping.UseMatchingCase());
            var config = MappingConfigs.Get<ICloneable>();
            config.Should().NotBeNull();
        }
    }

    class CaseCoverterWhichReturns : ICaseConverter
    {
        readonly string _returnValue;

        public CaseCoverterWhichReturns(string returnValue)
        {
            _returnValue = returnValue;
        }

        public string Convert(string s)
        {
            return _returnValue;
        }
    }
}
