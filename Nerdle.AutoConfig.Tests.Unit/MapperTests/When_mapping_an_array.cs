using System.Collections.Generic;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.MapperTests
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
            var result = _mapper.Map(xElement, typeof(int[]));
            result.Should().BeOfType<int[]>();
        }

        [Test]
        public void The_collection_items_are_added()
        {
            var xElement = XElement.Parse("<myArray><item>1</item><item>2</item></myArray>");
            var result = _mapper.Map(xElement, typeof(int[])) as int[];
            result.Should().BeEquivalentTo(1, 2);
        }
    }
}
