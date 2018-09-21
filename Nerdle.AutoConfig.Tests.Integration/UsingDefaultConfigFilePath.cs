using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class UsingDefaultConfigFilePath
    {
        [Test]
#if NETCOREAPP
        [Ignore("Buggy interaction between the test runner and System.Configuration.ConfigurationManager, see https://github.com/nunit/nunit3-vs-adapter/issues/356")]
#endif
        public void Mapping_from_the_default_file_path()
        {
            var foo = AutoConfig.Map<IFoo>();
            var bar = AutoConfig.Map<IFoo>("bar");
           
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
