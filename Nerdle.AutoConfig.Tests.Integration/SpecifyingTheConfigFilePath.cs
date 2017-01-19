using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class SpecifyingTheConfigFilePath
    {
        [Test]
        public void Mapping_from_a_specific_file_path()
        {
            var foo = AutoConfig.Map<IFoo>(configFilePath: "SpecifyingTheConfigFilePath.xml");
            var bar = AutoConfig.Map<IFoo>("bar", "SpecifyingTheConfigFilePath.xml");
           
            foo.Should().NotBeNull();
            foo.Name.Should().Be("foo");

            bar.Should().NotBeNull();
            bar.Name.Should().Be("bar");
        }

        public interface IFoo
        {
            string Name { get; }
        }
    }
}
