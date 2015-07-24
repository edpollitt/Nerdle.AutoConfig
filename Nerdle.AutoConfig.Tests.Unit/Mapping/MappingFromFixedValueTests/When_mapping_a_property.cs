using System;
using FluentAssertions;
using Nerdle.AutoConfig.Mapping;
using NUnit.Framework;

namespace Nerdle.AutoConfig.Tests.Unit.Mapping.MappingFromFixedValueTests
{
    [TestFixture]
    public class When_mapping_a_property
    {
        [TestCase(1)]
        [TestCase("hello")]
        [TestCase('x')]
        [TestCase(DayOfWeek.Monday)]
        public void The_value_is_applied(object value)
        {
            var propertyMapping = new MappingFromFixedValue(typeof(Foo).GetProperty("Bar"), value);
            var instance = new Foo();
            propertyMapping.Apply(instance);
            instance.Bar.Should().Be(value);
        }

        class Foo
        {
            public object Bar { get; set; }
        }
     }
}
