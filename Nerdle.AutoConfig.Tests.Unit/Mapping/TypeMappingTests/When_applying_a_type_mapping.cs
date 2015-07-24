using System.Collections.Generic;
using Moq;
using Nerdle.AutoConfig.Mapping;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mapping.TypeMappingTests
{
    [TestFixture]
    public class When_applying_a_type_mapping
    {
        [Test]
        public void All_the_included_property_mapping_are_applied()
        {
            var typeMapping = new TypeMapping();
            var propertyMappings = new List<Mock<IPropertyMapping>>();
            var instance = new object();

            for (int i = 0; i < 10; i++)
            {
                var propertyMapping = new Mock<IPropertyMapping>();
                propertyMappings.Add(propertyMapping);
                typeMapping.Include(propertyMapping.Object);
            }

            typeMapping.Apply(instance);

            for (int i = 0; i < 10; i++)
            {
                propertyMappings[i].Verify(pm => pm.Apply(instance), Times.Once);
            }
        }
    }
}
