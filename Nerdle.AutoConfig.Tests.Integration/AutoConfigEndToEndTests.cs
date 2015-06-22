using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    public class AutoConfigEndToEndTests
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

        [Test]
        public void Mapping_from_attributes()
        {
            var config = AutoConfig.Map<MappedFromAttributes>();
            config.Should().NotBeNull();
            config.Url.Should().Be("http://foo");
            config.Port.Should().Be(80);
        }

        [Test]
        public void Mapping_from_a_combination_of_properties_and_attributes()
        {
            var config = AutoConfig.Map<IMappedFromPropertiesAndAttributes>();
            config.Should().NotBeNull();
            config.Pizza.Should().Be("Vege Deluxe");
            config.Inches.Should().Be(12);
            config.Price.Should().Be(9.99M);
            config.Toppings.Should().HaveCount(3);
            config.Toppings.Should().ContainInOrder("Aubergine", "Spinach", "Artichoke");
        }

        [Test]
        public void Mapping_nested_complex_types()
        {
            var config = AutoConfig.Map<IHasNestedComplexTypes>();
            
            config.Should().NotBeNull();
            config.Foo.Should().Be("foo");
            config.Bar.Should().Be("bar");
            config.Baz.Should().Be("baz");
            
            config.Lunch.Should().NotBeNull();
            config.Lunch.Pizza.Should().Be("Vege Deluxe");
            config.Lunch.Inches.Should().Be(12);
            config.Lunch.Price.Should().Be(9.99M);
            config.Lunch.Toppings.Should().HaveCount(3);
            config.Lunch.Toppings.Should().ContainInOrder("Aubergine", "Spinach", "Artichoke");

            config.LunchServices.Should().NotBeNull();
            config.LunchServices.Should().HaveCount(2);
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


    public class MappedFromAttributes
    {
        public string Url { get; set; }
        public int Port { get; set; }
    }

    public interface IMappedFromPropertiesAndAttributes
    {
        string Pizza { get; }
        int Inches { get; }
        decimal Price { get; }
        IEnumerable<string> Toppings { get; } 
    }


    public interface IHasNestedComplexTypes
    {
        string Foo { get; }
        string Bar { get; }
        string Baz { get; }
        IMappedFromPropertiesAndAttributes Lunch { get; }
        IEnumerable<MappedFromAttributes> LunchServices { get; }
    }
}
