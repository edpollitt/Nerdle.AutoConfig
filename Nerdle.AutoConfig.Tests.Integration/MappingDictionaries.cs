using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class MappingDictionaries : EndToEndTest
    {
        [Test]
        public void Mapping_dictionaries()
        {
            var config = AutoConfig.Map<IDictionaryConfiguration>();
            config.Should().NotBeNull();
            config.FrenchWords.Should().HaveCount(3);
            config.FrenchWords.Keys.Should().ContainInOrder("cat", "dog", "monkey");
            config.FrenchWords.Values.Should().ContainInOrder("chat", "chien", "singe");
        }

        [Test]
        public void Mapping_complex_dictionaries()
        {
            var config = AutoConfig.Map<IComplexDictionaryConfiguration>();
            config.Should().NotBeNull();
            config.Endpoints.Should().HaveCount(2);
            config.Endpoints["Primary"].Should().HaveCount(2);
            config.Endpoints["Secondary"].Should().HaveCount(1);
        }
    }

    public interface IDictionaryConfiguration
    {
        IDictionary<string, string> FrenchWords { get; }
    }

    public interface IComplexDictionaryConfiguration
    {
        IDictionary<string, IEnumerable<IEndpointConfiguration>> Endpoints { get; }
    }

    public interface IEndpointConfiguration
    {
        string Name { get; }
        int Port { get; }
    }
}
