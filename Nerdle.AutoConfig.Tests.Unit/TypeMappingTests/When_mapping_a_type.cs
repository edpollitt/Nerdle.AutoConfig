using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Mappers;
using Nerdle.AutoConfig.Mappings;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.TypeMappingTests
{
    [TestFixture]
    public class When_mapping_a_type
    {
        readonly XElement _xElement = new XElement("testClass");
        readonly PropertyInfo _propertyInfo = typeof(TestClass).GetProperty("Foo");
            
        [Test]
        public void All_the_property_mappers_are_invoked()
        {
            var typeMapping = new TypeMapping();

            var mappers = new List<CountingMapper>();

            for (int i = 0; i < 10; i++)
            {
                var mapper = new CountingMapper();
                mappers.Add(mapper);
                typeMapping.Include(new ElementMapping(_xElement, _propertyInfo, mapper));
            }

            typeMapping.Apply(new TestClass());

            mappers.All(m => m.Invocations == 1).Should().BeTrue();
        }

        class TestClass
        {
            public bool Foo { get; set; }
        }

        class CountingMapper : IMapper
        {
            public int Invocations { get; private set; }

            public object Map(XElement element, Type type)
            {
                Invocations++;
                return null;
            }
        }
    }
}
