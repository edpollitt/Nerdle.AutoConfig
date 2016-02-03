using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class SectionNameConventions : EndToEndTest
    {
        [Test]
        public void Mapping_using_section_name_conventions()
        {
            var foo = AutoConfig.Map<IFooConfiguration>();
            var bar = AutoConfig.Map<IBarConfig>();
            var baz = AutoConfig.Map<IBazConfig>();
            var qux = AutoConfig.Map<IQuxConfiguration>();
            
            foo.Should().NotBeNull();
            foo.Name.Should().Be("foo");

            bar.Should().NotBeNull();
            bar.Name.Should().Be("bar");

            baz.Should().NotBeNull();
            baz.Name.Should().Be("baz");

            qux.Should().NotBeNull();
            qux.Name.Should().Be("qux");
        }

        public interface IFooConfiguration
        {
            string Name { get; }
        }

        public interface IBarConfig
        {
            string Name { get; }
        }

        public interface IBazConfig
        {
            string Name { get; }
        }

        public interface IQuxConfiguration
        {
            string Name { get; }
        }
    }
}