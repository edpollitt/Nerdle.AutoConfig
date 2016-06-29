using FluentAssertions;
using Nerdle.AutoConfig.Sections;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Sections.DefaultSectionNameConventionTests
{
    [TestFixture]
    class When_getting_alternative_names
    {
        readonly DefaultSectionNameConvention _sut = new DefaultSectionNameConvention();

        [TestCase("fooconfiguration", "foo")]
        [TestCase("fooConfiguration", "foo")]
        [TestCase("fooconfig", "foo")]
        [TestCase("fooConfig", "foo")]
        [TestCase("FOOCONFIG", "FOO")]
        [TestCase("fooConfigurationConfig", "fooConfiguration")]
        [TestCase("fooConfigConfiguration", "fooConfig")]
        public void Configuration_suffixes_are_removed(string name, string alternative)
        {
            _sut.GetAlternativeNames(name).Should().HaveCount(1).And.Contain(alternative);
        }

        [TestCase("fooConfigurationSettings")]
        [TestCase("Configuration")]
        [TestCase("config")]
        [TestCase("configObj")]
        public void No_alternatives_are_returned_if_a_configuration_suffix_is_not_matched(string name)
        {
            _sut.GetAlternativeNames(name).Should().BeEmpty();
        }
    }
}
