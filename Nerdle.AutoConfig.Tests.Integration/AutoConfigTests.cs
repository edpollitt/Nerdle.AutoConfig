using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class AutoConfigTests
    {
        [Test]
        public void Mapping_simple_types()
        {
            var config = AutoConfig.Map<IHasSimpleTypes>();
            config.Should().NotBeNull();
            config.String.Should().Be("hello world");
            config.Int.Should().Be(42);
            config.Nullable.Should().Be(55);
            config.EmptyNullable.Should().NotHaveValue();
            config.Bool.Should().BeTrue();
            config.Date.Should().Be(new DateTime(1969, 07, 21));
        }

        [Test]
        public void Mapping_collections()
        {
            var config = AutoConfig.Map<IHasCollections>();
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
            var config = AutoConfig.Map<IHasNestedCollections>();
            config.Should().NotBeNull();
            config.Numbers.Should().HaveCount(2);
            config.Numbers.First().Should().ContainInOrder(1, 3, 5);
            config.Numbers.Last().Should().ContainInOrder(2, 4);
        }
    }

    public interface IHasSimpleTypes
    {
        string String { get; }
        int Int { get; }
        int? Nullable { get; }
        int? EmptyNullable { get; }
        bool Bool { get; }
        DateTime Date { get; }
    }

    public interface IHasCollections
    {
        IEnumerable<long> Primes { get; }
        IList<ConsoleColor> Colors { get; }
        ICollection<string> Unicorns { get; } 
    }

    public interface IHasNestedCollections
    {
        IEnumerable<IEnumerable<int>> Numbers { get; } 
    }
}
