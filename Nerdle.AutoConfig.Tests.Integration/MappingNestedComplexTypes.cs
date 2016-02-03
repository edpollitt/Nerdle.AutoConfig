using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class MappingNestedComplexTypes : EndToEndTest
    {
        [Test]
        public void Mapping_nested_complex_types()
        {
            var config = AutoConfig.Map<IComplexConfiguration>();

            config.Should().NotBeNull();
            config.Foo.Should().Be("foo");
            config.Bar.Should().Be("bar");
            config.Baz.Should().Be("baz");

            config.SomeService.Should().NotBeNull();
            config.SomeService.Name.Should().Be("MyService");
            config.SomeService.RequestTimeout.Should().Be(20);
            config.SomeService.Enabled.Should().BeFalse();

            config.SomeOtherService.Should().NotBeNull();
            config.SomeOtherService.Name.Should().Be("MyOtherService");
            config.SomeOtherService.RequestTimeout.Should().Be(1000);
            config.SomeOtherService.Enabled.Should().BeTrue();

            config.SomeOtherService.Hosts.Should().NotBeNull();
            config.SomeOtherService.Hosts.Should().HaveCount(3);
            config.SomeOtherService.Hosts.Select(host => host.Name).Should().BeEquivalentTo("HOST-01", "HOST-02", "HOST-03");
            config.SomeOtherService.Hosts.Select(host => host.Port).All(port => port == 1700).Should().BeTrue();
        }

        public interface IComplexConfiguration
        {
            string Foo { get; }
            string Bar { get; }
            string Baz { get; }
            IServiceConfiguration SomeService { get; }
            IExtendedServiceConfiguration SomeOtherService { get; }
        }

        public interface IServiceConfiguration
        {
            string Name { get; }
            int RequestTimeout { get; }
            bool Enabled { get; }
        }

        public interface IExtendedServiceConfiguration : IServiceConfiguration
        {
            IEnumerable<IHost> Hosts { get; }
        }

        public interface IHost
        {
            string Name { get; }
            int Port { get; }
        }
    }
}
