using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class UsingDefaultConfigFilePath
#if NETCOREAPP
        : System.IDisposable
    {
        private readonly string _configFilePath;

        // In order to workaround flaky interaction between the test runner and System.Configuration.ConfigurationManager
        // See https://github.com/nunit/nunit3-vs-adapter/issues/356#issuecomment-700754225
        public UsingDefaultConfigFilePath()
        {
            _configFilePath = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None).FilePath;
            System.IO.File.Copy($"{System.Reflection.Assembly.GetExecutingAssembly().Location}.config", _configFilePath, overwrite: true);
        }

        public void Dispose()
        {
            System.IO.File.Delete(_configFilePath);
        }
#else
    {
#endif

        [Test]
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
