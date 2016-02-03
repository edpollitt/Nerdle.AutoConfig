using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class MappingCollections : EndToEndTest
    {
        [Test]
        public void Mapping_collections()
        {
            var config = AutoConfig.Map<ICollectionsConfiguration>();
            config.Should().NotBeNull();
            config.Primes.Should().HaveCount(4);
            config.Primes.Should().ContainInOrder(2L, 3L, 5L, 7L);
            config.Colors.Should().HaveCount(2);
            config.Colors.Should().ContainInOrder(ConsoleColor.Red, ConsoleColor.Green);
            config.Unicorns.Should().BeEmpty();
        }

        [Test]
        public void Mapping_nested_collections()
        {
            var config = AutoConfig.Map<INestedCollectionsConfiguration>();
            config.Should().NotBeNull();
            config.Numbers.Should().HaveCount(2);
            config.Numbers.First().Should().ContainInOrder(1, 3, 5);
            config.Numbers.Last().Should().ContainInOrder(2, 4);
        }

        [Test]
        public void Mapping_arrays()
        {
            var config = AutoConfig.Map<IArrayConfiguration>();
            config.Should().NotBeNull();
            config.Eggs.Should().HaveCount(3);
            config.Eggs.Should().ContainInOrder("fried", "scrambled", "poached");
        }

        public interface ICollectionsConfiguration
        {
            IEnumerable<long> Primes { get; }
            IList<ConsoleColor> Colors { get; }
            ICollection<string> Unicorns { get; }
        }

        public interface INestedCollectionsConfiguration
        {
            IEnumerable<ICollection<int>> Numbers { get; }
        }

        public interface IArrayConfiguration
        {
            string[] Eggs { get; }
        }
    }
}
