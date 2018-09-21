using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mapping.Mappers;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System;
using Nerdle.AutoConfig.Exceptions;

namespace Nerdle.AutoConfig.Tests.Unit.Mappers.DictionaryMapperTests
{
    [TestFixture]
    public class When_mapping_a_keyed_collection
    {
        readonly IMapper _sut = new KeyedCollectionMapper();

        [Test]
        public void The_keyed_collection_is_created()
        {
            var xElement = XElement.Parse("<myKeyedCollection></myKeyedCollection>");
            var result = _sut.Map(xElement, typeof(KeyedCollectionExample));
            result.Should().NotBeNull();
        }

        [Test]
        public void The_keyed_collection_is_the_correct_type()
        {
            var xElement = XElement.Parse("<myKeyedCollection></myKeyedCollection>");
            var result = _sut.Map(xElement, typeof(KeyedCollectionExample));
            result.Should().BeOfType<KeyedCollectionExample>();
        }

        [Test]
        public void Items_can_be_added()
        {
            var xElement = XElement.Parse("<myKeyedCollection><item>1</item><item>2</item></myKeyedCollection>");
            var result = _sut.Map(xElement, typeof(KeyedCollectionExample)) as KeyedCollectionExample;
            result.Should().HaveCount(2);
            result["1"].Should().Be(1);
            result["2"].Should().Be(2);
        }

        [Test]
        public void The_type_must_be_instantiable()
        {
            var xElement = XElement.Parse("<myKeyedCollection></myKeyedCollection>");
            Action mapping = () => _sut.Map(xElement, typeof(KeyedCollection<char, byte>));
            mapping.Should().Throw<AutoConfigMappingException>()
                .Where(e => e.Message.Contains(typeof(KeyedCollection<char, byte>).FullName)
                 && e.Message.Contains("is not instantiable"));
        }

        [Test]
        public void The_type_must_have_a_parameterless_constructor()
        {
            var xElement = XElement.Parse("<myKeyedCollection></myKeyedCollection>");
            Action mapping = () => _sut.Map(xElement, typeof(KeyedCollectionWithNoParameterlessConstructorExample));
            mapping.Should().Throw<AutoConfigMappingException>()
                .Where(e => e.Message.Contains(typeof(KeyedCollectionWithNoParameterlessConstructorExample).FullName)
                 && e.Message.Contains("no parameterless constructor"));
        }

        class KeyedCollectionExample : KeyedCollection<string, int>
        {
            protected override string GetKeyForItem(int item)
            {
                return item.ToString();
            }
        }

        class KeyedCollectionWithNoParameterlessConstructorExample : KeyedCollectionExample
        {
            public KeyedCollectionWithNoParameterlessConstructorExample(int i)
            { }
        }
    }
}
