using System.Collections.Generic;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mappers.CollectionMapperTests
{
    [TestFixture]
    public class When_mapping_a_collection
    {
        readonly IMapper _mapper = new CollectionMapper();

        [Test]
        public void The_collection_is_created()
        {
            var xElement = XElement.Parse("<myList></myList>");
            var result = _mapper.Map(xElement, typeof (ICollection<int>));
            result.Should().NotBeNull();
        }

        [Test]
        public void The_collection_is_the_correct_type()
        {
            var xElement = XElement.Parse("<myList></myList>");
            var result = _mapper.Map(xElement, typeof(IList<decimal>));
            result.Should().BeAssignableTo<IList<decimal>>();
        }

        [Test]
        public void The_collection_items_are_added()
        {
            var xElement = XElement.Parse("<myList><item>1</item><item>2</item></myList>");
            var result = _mapper.Map(xElement, typeof(List<int>)) as List<int>;
            result.Should().BeEquivalentTo(1, 2);
        }
    }
}
