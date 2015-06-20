using System;
using System.Xml.Linq;
using FluentAssertions;
using Nerdle.AutoConfig.Exceptions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.TypeMappingTests
{
    [TestFixture]
    public class When_creating_a_mapping
    {
        [Test]
        public void A_mapping_is_created_if_all_properties_and_elements_match()
        {
            var theElement = XElement.Parse("<Test><Foo>1</Foo><Bar>1</Bar></Test>");
            var theMapping = TypeMapping.CreateFor<TestClass>(theElement);
            theMapping.Should().NotBeNull();
        }

        [Test]
        public void An_exception_is_thrown_if_a_property_is_not_matched()
        {
            var theElement = XElement.Parse("<Test><Foo>1</Foo></Test>");
            Action creatingTheMapping = () => TypeMapping.CreateFor<TestClass>(theElement);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .WithMessage("Could not map property 'Bar' for type Nerdle.AutoConfig.Tests.TypeMappingTests.TestClass from section 'Test'. No matching config element was founnd.");
        }

        [Test]
        public void An_exception_is_thrown_if_an_element_is_not_matched()
        {
            var theElement = XElement.Parse("<Test><Foo>1</Foo><Bar>1</Bar><Baz>1</Baz></Test>");
            Action creatingTheMapping = () => TypeMapping.CreateFor<TestClass>(theElement);
            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
                .WithMessage("Could not map type Nerdle.AutoConfig.Tests.TypeMappingTests.TestClass from section 'Test'. No matching property for config element 'Baz' was founnd.");
        }
    }


    class TestClass
    {
        public int Foo { get; set; }
        public int Bar { get; set; }
    }
}
