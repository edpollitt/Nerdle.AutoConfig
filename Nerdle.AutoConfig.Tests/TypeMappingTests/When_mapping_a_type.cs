using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.TypeMappingTests
{
    [TestFixture]
    public class When_mapping_a_type
    {
        readonly XElement _xElement = new XElement("testClass");
        readonly PropertyInfo _propertyInfo = typeof(TestClass).GetProperty("BoolProperty");
            
        [Test]
        public void All_the_property_mappers_are_invoked()
        {
            var typeMapping = new TypeMapping<TestClass>();

            var mappers = new List<CountingMapper>();

            for (int i = 0; i < 10; i++)
            {
                var mapper = new CountingMapper();
                mappers.Add(mapper);
                typeMapping.Include(new PropertyMapping<TestClass>(_xElement, _propertyInfo, mapper));
            }

            typeMapping.Apply(new TestClass());

            mappers.All(m => m.Invocations == 1).Should().BeTrue();
        }

        class TestClass
        {
            public bool BoolProperty { get; set; }
        }

        class CountingMapper : IMapper<TestClass>
        {
            public int Invocations { get; set; }

            public void Map(XElement element, PropertyInfo property, TestClass instance)
            {
                Invocations++;
            }
        }
    }
}
