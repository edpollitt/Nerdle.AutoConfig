using System.Collections.Generic;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mapping.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mappers.CollectionMapperTests
{
    [TestFixture]
    public class When_mapping_a_collection
    {
        readonly IMapper _sut = new CollectionMapper();

        [Test]
        public void The_collection_is_created()
        {
            var xElement = XElement.Parse("<myCollection></myCollection>");
            var result = _sut.Map(xElement, typeof (ICollection<int>));
            result.Should().NotBeNull();
        }

        [Test]
        public void The_collection_is_the_correct_type()
        {
            var xElement = XElement.Parse("<myList></myList>");
            var result = _sut.Map(xElement, typeof(IList<decimal>));
            result.Should().BeAssignableTo<IList<decimal>>();
        }

        [Test]
        public void The_collection_items_are_added()
        {
            var xElement = XElement.Parse("<myEnumerable><item>1</item><item>2</item></myEnumerable>");
            var result = _sut.Map(xElement, typeof(IEnumerable<int>)) as IEnumerable<int>;
            result.Should().BeEquivalentTo(1, 2);
        }
    }
}
