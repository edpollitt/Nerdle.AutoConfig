using System.Collections.Generic;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mappers.DictionaryMapperTests
{
    [TestFixture]
    public class When_mapping_a_dictionary
    {
        readonly IMapper _mapper = new DictionaryMapper();

        [Test]
        public void The_dictionary_is_created()
        {
            var xElement = XElement.Parse("<myDictionary></myDictionary>");
            var result = _mapper.Map(xElement, typeof(Dictionary<int, int>));
            result.Should().NotBeNull();
        }

        [Test]
        public void The_dictionary_is_the_correct_type()
        {
            var xElement = XElement.Parse("<myDictionary></myDictionary>");
            var result = _mapper.Map(xElement, typeof(Dictionary<int, bool>));
            result.Should().BeOfType<Dictionary<int, bool>>();
        }

        [Test]
        public void The_dictionary_items_are_added()
        {
            var xElement = XElement.Parse("<myDictionary><item key=\"1\" value=\"one\"/></myDictionary>");
            var result = _mapper.Map(xElement, typeof(Dictionary<int, string>)) as Dictionary<int, string>;
            result.Should().HaveCount(1);
            result[1].Should().Be("one");
        }

        [Test]
        public void Dictionary_items_can_be_added_using_elements()
        {
            var xElement = XElement.Parse("<myDictionary><item><key>1</key><value>one</value></item></myDictionary>");
            var result = _mapper.Map(xElement, typeof(Dictionary<int, string>)) as Dictionary<int, string>;
            result.Should().HaveCount(1);
            result[1].Should().Be("one");
        }
    }
}
