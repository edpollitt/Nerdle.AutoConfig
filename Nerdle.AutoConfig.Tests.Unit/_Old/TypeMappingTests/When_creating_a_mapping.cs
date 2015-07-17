//using System;
//using System.Xml.Linq;
//using FluentAssertions;
//using Nerdle.AutoConfig.Exceptions;
//using Nerdle.AutoConfig.Mappings;
//using NUnit.Framework;

//namespace Nerdle.AutoConfig.Tests.Unit.TypeMappingTests
//{
//    [TestFixture]
//    public class When_creating_a_mapping
//    {
//        [Test]
//        public void A_mapping_is_created_if_all_properties_and_elements_match()
//        {
//            var xElement = XElement.Parse("<test><foo>1</foo><bar>1</bar></test>");
//            var mapping = TypeMapping.CreateFor(typeof(TestClass), xElement);
//            mapping.Should().NotBeNull();
//        }

//        [Test]
//        public void An_exception_is_thrown_if_a_property_is_not_matched()
//        {
//            var xElement = XElement.Parse("<test><foo>1</foo></test>");
//            Action creatingTheMapping = () => TypeMapping.CreateFor(typeof(TestClass), xElement);
//            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
//                .WithMessage("Could not map property 'Bar' for type 'Nerdle.AutoConfig.Tests.Unit.TypeMappingTests.TestClass' from section 'Test'. No matching config element or attribute was found.");
//        }

//        [Test]
//        public void Properties_can_be_matched_from_attributes_or_elements()
//        {
//            var xElement = XElement.Parse("<test foo=\"1\"><bar>1</bar></test>");
//            var mapping = TypeMapping.CreateFor(typeof(TestClass), xElement);
//            mapping.Should().NotBeNull();
//        }

//        [Test]
//        public void An_exception_is_thrown_if_an_element_is_not_matched()
//        {
//            var xElement = XElement.Parse("<test><foo>1</foo><bar>1</bar><baz>1</baz></test>");
//            Action creatingTheMapping = () => TypeMapping.CreateFor(typeof(TestClass), xElement);
//            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
//                .WithMessage("Could not map type 'Nerdle.AutoConfig.Tests.Unit.TypeMappingTests.TestClass' from section 'Test'. No matching settable property for config element 'Baz' was found.");
//        }

//        [Test]
//        public void An_exception_is_thrown_if_an_attribute_is_not_matched()
//        {
//            var xElement = XElement.Parse("<test foo=\"1\" bar=\"1\" baz=\"1\" />");
//            Action creatingTheMapping = () => TypeMapping.CreateFor(typeof(TestClass), xElement);
//            creatingTheMapping.ShouldThrow<AutoConfigMappingException>()
//                .WithMessage("Could not map type 'Nerdle.AutoConfig.Tests.Unit.TypeMappingTests.TestClass' from section 'Test'. No matching settable property for config attribute 'Baz' was found.");
//        }
//    }


//    class TestClass
//    {
//        public int Foo { get; set; }
//        public int Bar { get; set; }
//    }
//}
