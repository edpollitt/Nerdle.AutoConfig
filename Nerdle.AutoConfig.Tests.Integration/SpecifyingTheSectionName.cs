using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class SpecifyingTheSectionName : EndToEndTest
    {
        [Test]
        public void Mapping_from_specified_sections()
        {
            var foo = AutoConfig.Map<IFooBarBaz>("foo");
            var bar = AutoConfig.Map<IFooBarBaz>("bar");
            var baz = AutoConfig.Map<IFooBarBaz>("baz");
            var bazUpper = AutoConfig.Map<IFooBarBaz>("BAZ");

            foo.Should().NotBeNull();
            foo.Name.Should().Be("foo");

            bar.Should().NotBeNull();
            bar.Name.Should().Be("bar");

            baz.Should().NotBeNull();
            baz.Name.Should().Be("baz");

            bazUpper.Should().NotBeNull();
            bazUpper.Name.Should().Be("BAZ");
        }
    }

    public interface IFooBarBaz
    {
        string Name { get; }
    }

    public interface IFooConfiguration
    {
        string Name { get; }
    }

    public interface IBarConfig
    {
        string Name { get; }
    }
}
