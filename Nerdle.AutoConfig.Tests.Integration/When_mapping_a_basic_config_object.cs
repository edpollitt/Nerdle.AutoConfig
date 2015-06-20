using System;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class When_mapping_a_basic_config_object
    {
        [Test]
        public void It_works()
        {
            var config = AutoConfig.Map<IBasicConfig>();
            config.Should().NotBeNull();
            config.String.Should().Be("hello world");
            config.Int.Should().Be(42);
            config.Bool.Should().BeTrue();
            config.Date.Should().Be(new DateTime(1969, 07, 21));
        }
    }

    public interface IBasicConfig
    {
        string String { get; }
        int Int { get; }
        bool Bool { get; }
        DateTime Date { get; }
    }
}
