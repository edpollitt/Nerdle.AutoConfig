using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mappers.ArrayMapperTests
{
    [TestFixture]
    public class When_mapping_an_array
    {
        readonly IMapper _mapper = new ArrayMapper();

        [Test]
        public void The_array_is_created()
        {
            var xElement = XElement.Parse("<myArray></myArray>");
            var result = _mapper.Map(xElement, typeof(int[]));
            result.Should().NotBeNull();
        }

        [Test]
        public void The_collection_is_the_correct_type()
        {
            var xElement = XElement.Parse("<myArray></myArray>");
            var result = _mapper.Map(xElement, typeof(float[]));
            result.Should().BeOfType<float[]>();
        }

        [Test]
        public void The_collection_items_are_added()
        {
            var xElement = XElement.Parse("<animals><animal>dog</animal><animal>cat</animal></animals>");
            var result = _mapper.Map(xElement, typeof(string[])) as string[];
            result.Should().BeEquivalentTo("dog", "cat");
        }
    }
}
