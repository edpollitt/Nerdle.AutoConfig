using System;
using FluentAssertions;
using Nerdle.AutoConfig.Casing;
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
            mappingConfig.Case.Convert("something").Should().Be("foobar");
        }

        [Test]
        public void A_mapping_can_be_configured_for_a_particular_type()
        {
            AutoConfig.WhenMapping<ICloneable>(mapping => mapping.UseMatchingCase());
            var config = MappingConfigs.GetFor<ICloneable>();
            config.Should().NotBeNull();
            config.Case.Should().BeOfType<MatchingCase>();
        }

        [Test]
        public void A_default_mapping_is_used_if_no_mapping_is_explicitly_configured()
        {
            var config = MappingConfigs.GetFor<IFormattable>();
            config.Should().NotBeNull();
            config.Case.Should().BeOfType<CamelCase>();
        }
    }

    class CaseCoverterWhichReturns : ICase
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
