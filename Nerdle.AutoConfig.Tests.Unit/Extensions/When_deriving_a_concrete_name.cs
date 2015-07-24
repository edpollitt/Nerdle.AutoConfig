using System;
using System.ComponentModel;
using System.Security.Principal;
using FluentAssertions;
using Nerdle.AutoConfig.Extensions;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Extensions
{
    [TestFixture]
    class When_deriving_a_concrete_name
    {
        [TestCase(typeof(string), "String")]
        [TestCase(typeof(InheritanceLevel), "InheritanceLevel")]
        [TestCase(typeof(IndexOutOfRangeException), "IndexOutOfRangeException")]
        [TestCase(typeof(CombinatorialAttribute), "CombinatorialAttribute")]
        public void Return_the_type_name_if_the_type_is_an_object_type(Type type, string expectedResult)
        {
            type.ConcreteName().Should().Be(expectedResult);
        }

        [TestCase(typeof(ICloneable), "Cloneable")]
        [TestCase(typeof(IBindingList), "BindingList")]
        [TestCase(typeof(IIdentity), "Identity")]
        [TestCase(typeof(IIntellisenseBuilder), "IntellisenseBuilder")]
        public void Return_the_type_name_without_the_leading_I_if_the_type_is_an_interface_type_and_follows_standard_naming_convention(Type type, string expectedResult)
        {
            type.ConcreteName().Should().Be(expectedResult);
        }

        [TestCase(typeof(FooInterface), "FooInterface")]
        [TestCase(typeof(InterfaceFoo), "InterfaceFoo")]
        public void Return_the_type_name_exactly_if_the_type_is_an_interface_type_but_does_not_follow_standard_naming_convention(Type type, string expectedResult)
        {
            type.ConcreteName().Should().Be(expectedResult);
        }

        interface FooInterface { }
        interface InterfaceFoo { }
    }
}
