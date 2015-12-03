using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class SectionNameConventions : EndToEndTest
    {
        [Test]
        public void Mapping_removing_the_suffix_by_convention()
        {
            var foo = AutoConfig.Map<IFooConfiguration>();
            var bar = AutoConfig.Map<IBarConfig>();
            
            foo.Should().NotBeNull();
            foo.Name.Should().Be("fooConfiguration");

            bar.Should().NotBeNull();
            bar.Name.Should().Be("barConfig");
        }
    }
}