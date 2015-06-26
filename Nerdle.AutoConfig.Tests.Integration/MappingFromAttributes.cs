using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Integration
{
    [TestFixture]
    class MappingFromAttributes : EndToEndTest
    {
        [Test]
        public void Mapping_from_attributes()
        {
            var config = AutoConfig.Map<EndPointConfiguration>();
            config.Should().NotBeNull();
            config.Url.Should().Be("http://foo");
            config.Port.Should().Be(80);
        }

        [Test]
        public void Mapping_from_a_combination_of_properties_and_attributes()
        {
            var config = AutoConfig.Map<IPizzaConfiguration>();
            config.Should().NotBeNull();
            config.Name.Should().Be("Vege Deluxe");
            config.Base.Should().Be("Stuffed Crust");
            config.Size.Should().Be(PizzaSize.XLarge);
            config.Price.Should().Be(9.99M);
            config.Toppings.Should().HaveCount(3);
            config.Toppings.Should().ContainInOrder("Aubergine", "Spinach", "Artichoke");
        }
    }

    public class EndPointConfiguration
    {
        public string Url { get; set; }
        public int Port { get; set; }
    }

    public interface IPizzaConfiguration
    {
        string Name { get; }
        string Base { get; }
        PizzaSize Size { get; }
        decimal Price { get; }
        IEnumerable<string> Toppings { get; }
    }

    public enum PizzaSize
    {
        Large,
        XLarge,
    }
}
