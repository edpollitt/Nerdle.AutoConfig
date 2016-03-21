using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class MappingWithDefaultConfig
    {
        [Test]
        public void If_the_named_config_section_is_not_found_the_default_is_used()
        {
            var defaultConfig = new MyConfiguration { Name = "config", Value = 1 };
            var result = AutoConfig.MapWithDefault(defaultConfig, "myConfigType");

            result.Should().Be(defaultConfig);
        }

        [Test]
        public void If_the_unanmed_config_section_is_not_found_the_default_is_used()
        {
            var defaultConfig = new MyConfiguration { Name = "config", Value = 1 };
            var result = AutoConfig.MapWithDefault(defaultConfig);

            result.Should().Be(defaultConfig);
        }
    }

    class MyConfiguration
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
