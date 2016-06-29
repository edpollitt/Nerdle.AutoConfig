using Moq;
using Nerdle.AutoConfig.Mapping;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mapping.TypeMappingTests
{
    [TestFixture]
    public class When_applying_a_type_mapping
    {
        [Test]
        public void All_the_included_property_mappings_are_applied()
        {
            var sut = new TypeMapping();
            var propertyMappings = new Mock<IPropertyMapping>[10];
            var instance = new object();

            for (int i = 0; i < propertyMappings.Length; i++)
            {
                var propertyMapping = new Mock<IPropertyMapping>();
                propertyMappings[i] = (propertyMapping);
                sut.Include(propertyMapping.Object);
            }

            sut.Apply(instance);

            for (int i = 0; i < propertyMappings.Length; i++)
            {
                propertyMappings[i].Verify(pm => pm.Apply(instance), Times.Once);
            }
        }
    }
}
